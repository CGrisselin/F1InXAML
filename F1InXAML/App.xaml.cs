using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace F1InXAML
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {    
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
        }
    }
}
