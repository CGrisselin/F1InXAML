using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Squirrel;

namespace F1InXAML
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Task<UpdateManager> _updateManager = null;


        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            //Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline),
            //    new FrameworkPropertyMetadata { DefaultValue = 30 });

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof (FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));           

            var mainWindow = new MainWindow {DataContext = new MainViewModel()};
            mainWindow.Show();

            Task.Factory.StartNew(CheckForUpdates);           
        }

        private static async void CheckForUpdates()
        {              
            _updateManager = UpdateManager.GitHubUpdateManager("https://github.com/MaterialDesignInXAML/F1InXAML", "F1ix");

            if (_updateManager.Result.IsInstalledApp)
                await _updateManager.Result.UpdateApp();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _updateManager?.Dispose();
        }
    }
}
