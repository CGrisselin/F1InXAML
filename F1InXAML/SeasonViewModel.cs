using System;
using System.ComponentModel;

namespace F1InXAML
{
    public class SeasonViewModel : INotifyPropertyChanged
    {        
        private Season _season;
        private int _selectedTabIndex;

        public void Show(Season season)
        {
            if (season == null) throw new ArgumentNullException(nameof(season));

            SelectedTabIndex = 0;
            Season = season;
        }

        public Season Season
        {
            get { return _season; }
            private set { this.MutateVerbose(ref _season, value, RaisePropertyChanged()); }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { this.MutateVerbose(ref _selectedTabIndex, value, RaisePropertyChanged()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}