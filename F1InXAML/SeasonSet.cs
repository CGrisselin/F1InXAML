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

        public SeasonSet(Action<Season> showSeasonHandler)
        {
            if (showSeasonHandler == null) throw new ArgumentNullException(nameof(showSeasonHandler));

            Seasons = new ReadOnlyObservableCollection<Season>(_seasons);
            ShowSeasonCommand = new Command(o =>
            {
                var season = o as Season;
                if (season != null)
                    showSeasonHandler(season);
            });

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

            for (var i = 2015; i >= 2010; i--)
            {
                var season = await FetchAsync(i);
                _seasons.Add(season);
            }

            IsBusy = false;
        }

        private static async Task<Season> FetchAsync(int year)
        {
            var driverStandings = await GetDriverStandings(year);
            var constructorStandings = await GetConstructorStandings(year);

            return new Season(year, driverStandings, constructorStandings);
        }

        private static async Task<DriverStanding[]> GetDriverStandings(int year)
        {
            var webRequest = CreateWebRequest($"http://ergast.com/api/f1/{year}/driverStandings");

            var webResponse = await webRequest.GetResponseAsync();
            using (var stream = webResponse.GetResponseStream())
            {
                XNamespace ns;
                var xDocument = ReadResponse(stream, out ns);

                var driverStandings = xDocument.Root
                    .Element(ns + "StandingsTable")
                    .Element(ns + "StandingsList")
                    .Elements(ns + "DriverStanding").OrderBy(standingElement => standingElement.Attribute("position").Value)
                    .Select(standingElement => new DriverStanding(ToDriver(standingElement.Element(ns + "Driver"), ns),
                        int.Parse(standingElement.Attribute("position").Value),
                        int.Parse(standingElement.Attribute("points").Value),
                        int.Parse(standingElement.Attribute("wins").Value)));

                return driverStandings.OrderBy(ds => ds.Position).ToArray();
            }
        }

        private static async Task<ConstructorStanding[]> GetConstructorStandings(int year)
        {
            var webRequest = CreateWebRequest($"http://ergast.com/api/f1/{year}/constructorStandings");

            var webResponse = await webRequest.GetResponseAsync();
            using (var stream = webResponse.GetResponseStream())
            {
                XNamespace ns;
                var xDocument = ReadResponse(stream, out ns);

                var constructorStandings = xDocument.Root
                    .Element(ns + "StandingsTable")
                    .Element(ns + "StandingsList")
                    .Elements(ns + "ConstructorStanding").OrderBy(standingElement => standingElement.Attribute("position").Value)
                    .Select(standingElement => new ConstructorStanding(ToConstructor(standingElement.Element(ns + "Constructor"), ns),
                        int.Parse(standingElement.Attribute("position").Value),
                        int.Parse(standingElement.Attribute("points").Value),
                        int.Parse(standingElement.Attribute("wins").Value)));

                return constructorStandings.OrderBy(ds => ds.Position).ToArray();
            }
        }

        private static XDocument ReadResponse(Stream stream, out XNamespace ns)
        {
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            var xml = streamReader.ReadToEnd();
            var xDocument = XDocument.Parse(xml);
            ns = xDocument.Root.GetDefaultNamespace();
            return xDocument;
        }

        private static WebRequest CreateWebRequest(string url)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.UseDefaultCredentials = true;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
            return webRequest;
        }

        private static Driver ToDriver(XElement driverElement, XNamespace ns)
        {
            return new Driver(
                driverElement.Attribute("driverId").Value,
                driverElement.Attribute("code").Value,
                driverElement.Attribute("url").Value,
                driverElement.ElementValueOrNull(ns + "PermanentNumber"),
                driverElement.ElementValueOrNull(ns + "GivenName"),
                driverElement.ElementValueOrNull(ns + "FamilyName"),
                driverElement.ElementValueOrNull(ns + "DateOfBirth"),
                driverElement.ElementValueOrNull(ns + "Nationality"));
        }

        private static Constructor ToConstructor(XElement driverElement, XNamespace ns)
        {
            return new Constructor(
                driverElement.Attribute("constructorId").Value,                
                driverElement.Attribute("url").Value,                
                driverElement.ElementValueOrNull(ns + "Name"),
                driverElement.ElementValueOrNull(ns + "Nationality"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}