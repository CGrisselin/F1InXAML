using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace F1InXAML
{
    public class SeasonSet : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Season> _seasons = new ObservableCollection<Season>();
        private bool _isBusy;

        public SeasonSet()
        {
            Seasons = new ReadOnlyObservableCollection<Season>(_seasons);

            //TODO error handle
            Init();
        }

        public ICommand ShowSeasonCommand { get; }

        public ReadOnlyObservableCollection<Season> Seasons { get; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { this.MutateVerbose(ref _isBusy, value, RaisePropertyChanged()); }
        }

        private async void Init()
        {
            IsBusy = true;

            var seasonHeaders = await FetchAsync();
            foreach (var seasonHeader in seasonHeaders.Where(sh => sh.Year >= 2000))
            {
                var season = await FetchAsync(seasonHeader);
                _seasons.Add(season);
            }

            IsBusy = false;
        }

        private static async Task<SeasonHeader[]> FetchAsync()
        {
            var webRequest = ApiHelper.CreateWebRequest("http://ergast.com/api/f1/seasons?limit=1000");
            var webResponse = await webRequest.GetResponseAsync();
            using (var stream = webResponse.GetResponseStream())
            {
                XNamespace ns;
                var xDocument = ApiHelper.ReadResponse(stream, out ns);

                return xDocument.Root
                    .Element(ns + "SeasonTable")
                    .Elements(ns + "Season")
                    .Select(XExtensions.ToSeasonHeader)
                    .OrderByDescending(sh => sh.Year)
                    .ToArray();
            }
        }

        private static async Task<Season> FetchAsync(SeasonHeader seasonHeader)
        {
            var calendar = await GetCalendar(seasonHeader.Year);
            var driverStandings = await GetDriverStandings(seasonHeader.Year);
            var constructorStandings = await GetConstructorStandings(seasonHeader.Year);

            return new Season(seasonHeader.Year, seasonHeader.Url, calendar, driverStandings, constructorStandings);
        }

        private static async Task<Race[]> GetCalendar(int year)
        {
            var webRequest = ApiHelper.CreateWebRequest($"http://ergast.com/api/f1/{year}");

            var webResponse = await webRequest.GetResponseAsync();
            using (var stream = webResponse.GetResponseStream())
            {
                XNamespace ns;
                var xDocument = ApiHelper.ReadResponse(stream, out ns);

                return xDocument.Root.Element(ns + "RaceTable")
                    .Elements(ns + "Race")
                    .Select(raceElement => raceElement.ToRace(ns))
                    .ToArray();
            }
        }        

        private static async Task<DriverStanding[]> GetDriverStandings(int year)
        {
            var webRequest = ApiHelper.CreateWebRequest($"http://ergast.com/api/f1/{year}/driverStandings");

            var webResponse = await webRequest.GetResponseAsync();
            using (var stream = webResponse.GetResponseStream())
            {
                XNamespace ns;
                var xDocument = ApiHelper.ReadResponse(stream, out ns);

                var standingsList = xDocument.Root.Element(ns + "StandingsTable").Element(ns + "StandingsList");
                if (standingsList == null) return new DriverStanding[0];

                var driverStandings = standingsList
                    .Elements(ns + "DriverStanding").OrderBy(standingElement => standingElement.Attribute("position").Value)
                    .Select(standingElement => new DriverStanding(standingElement.Element(ns + "Driver").ToDriver(ns),
                        int.Parse(standingElement.Attribute("position").Value),
                        double.Parse(standingElement.Attribute("points").Value),
                        int.Parse(standingElement.Attribute("wins").Value)));

                return driverStandings.OrderBy(ds => ds.Position).ToArray();
            }
        }

        private static async Task<ConstructorStanding[]> GetConstructorStandings(int year)
        {
            var webRequest = ApiHelper.CreateWebRequest($"http://ergast.com/api/f1/{year}/constructorStandings");

            var webResponse = await webRequest.GetResponseAsync();
            using (var stream = webResponse.GetResponseStream())
            {
                XNamespace ns;
                var xDocument = ApiHelper.ReadResponse(stream, out ns);

                var standingsList = xDocument.Root.Element(ns + "StandingsTable").Element(ns + "StandingsList");
                if (standingsList == null) return new ConstructorStanding[0];

                var constructorStandings = standingsList
                    .Elements(ns + "ConstructorStanding").OrderBy(standingElement => standingElement.Attribute("position").Value)
                    .Select(standingElement => new ConstructorStanding(standingElement.Element(ns + "Constructor").ToConstructor(ns),
                        int.Parse(standingElement.Attribute("position").Value),
                        double.Parse(standingElement.Attribute("points").Value),
                        int.Parse(standingElement.Attribute("wins").Value)));

                return constructorStandings.OrderBy(ds => ds.Position).ToArray();
            }
        }                

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}