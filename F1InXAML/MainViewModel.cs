using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace F1InXAML
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Season _activeSeason;
        private int _activePageIndex;

        public SeasonSet SeasonSet => new SeasonSet(ShowSeasonHandler);

        private void ShowSeasonHandler(Season season)
        {
            ActiveSeason = season;
            ActivePageIndex = 1;
        }

        public Season ActiveSeason
        {
            get { return _activeSeason; }
            private set { this.MutateVerbose(ref _activeSeason, value, RaisePropertyChanged()); }
        }

        public int ActivePageIndex
        {
            get { return _activePageIndex; }
            set { this.MutateVerbose(ref _activePageIndex, value, RaisePropertyChanged()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
