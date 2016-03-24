using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace F1InXAML
{
    public class RaceViewModel : INotifyPropertyChanged, ITidyable
    {
        private bool _isBusy;
        private Race _race;
        private IReadOnlyCollection<Result> _results;
        private int _selectedTabIndex;

        public void Show(Race race)
        {
            if (race == null) throw new ArgumentNullException(nameof(race));

            Init(race);
        }

        public void Tidy()
        {
            Race = null;
            Results = new Result[0];
            SelectedTabIndex = 0;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set { this.MutateVerbose(ref _isBusy, value, RaisePropertyChanged()); }
        }

        public Race Race
        {
            get { return _race; }
            private set { this.MutateVerbose(ref _race, value, RaisePropertyChanged()); }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { this.MutateVerbose(ref _selectedTabIndex, value, RaisePropertyChanged()); }
        }


        private async void Init(Race race)
        {
            IsBusy = true;            

            var webRequest = ApiHelper.CreateWebRequest($"http://ergast.com/api/f1/{race.Season}/{race.Round}/results");
            var webResponse = await webRequest.GetResponseAsync();
            using (var stream = webResponse.GetResponseStream())
            {
                XNamespace ns;
                var xDocument = ApiHelper.ReadResponse(stream, out ns);
                var raceElement = xDocument.Root.Element(ns + "RaceTable").Element(ns + "Race");
                var raceResults = raceElement?.ToRaceResults(ns);
                Race = raceResults?.Race ?? race;
                Results = raceResults?.Results;
            }

            IsBusy = false;
        }

        public IReadOnlyCollection<Result> Results
        {
            get { return _results; }
            private set { this.MutateVerbose(ref _results, value, RaisePropertyChanged()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}