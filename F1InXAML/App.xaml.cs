using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace F1InXAML
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {    
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow {DataContext = new MainViewModel()};
            mainWindow.Show();
        }
    }
}
