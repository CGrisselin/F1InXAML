using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Markup;

namespace F1InXAML
{
    public class MainViewModel : INotifyPropertyChanged, ISlideNavigationSubject
    {
        private readonly SlideNavigator _slideNavigator;
        private int _activeSlideIndex;

        public MainViewModel()
        {
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(NavigationCommands.ShowRaceCommand, ShowRaceExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(NavigationCommands.ShowSeasonCommand, ShowSeasonExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(NavigationCommands.GoBackCommand, GoBackExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(System.Windows.Input.NavigationCommands.BrowseBack, GoBackExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(System.Windows.Input.NavigationCommands.BrowseForward, GoForwardExecuted));

            Slides = new object[] {SeasonSet, SeasonViewModel, RaceViewModel};
            _slideNavigator = new SlideNavigator(this, Slides);
            _slideNavigator.GoTo(0);
        }

        public object[] Slides { get; }

        public SeasonSet SeasonSet { get; } = new SeasonSet();

        public RaceViewModel RaceViewModel { get; } = new RaceViewModel();

        public SeasonViewModel SeasonViewModel { get; } = new SeasonViewModel();

        public string Version { get; } = Assembly.GetEntryAssembly().GetName().Version.ToString();

        public int ActiveSlideIndex
        {
            get { return _activeSlideIndex; }
            set { this.MutateVerbose(ref _activeSlideIndex, value, RaisePropertyChanged()); }
        }

        private void ShowSeasonExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _slideNavigator.GoTo(
                IndexOfSlide<SeasonViewModel>(),
                () => SeasonViewModel.Show((Season)e.Parameter));
        }

        private void ShowRaceExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _slideNavigator.GoTo(
                IndexOfSlide<RaceViewModel>(),
                () => RaceViewModel.Show((Race)e.Parameter));
        }

        private void GoBackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _slideNavigator.GoBack();
        }

        private void GoForwardExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _slideNavigator.GoForward();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        private int IndexOfSlide<TSlide>()
        {
            return Slides.Select((o, i) => new {o, i}).First(a => a.o.GetType() == typeof (TSlide)).i;
        }
    }
}
