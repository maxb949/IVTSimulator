using IvtSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IvtSimulator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel dataContext;

        private void OnStartup(object sender, StartupEventArgs e)
        {
            dataContext = new MainViewModel();

            var window = new MainWindow()
            {
                DataContext = dataContext,
            };
            MainWindow = window;
            MainWindow.Show();
        }

    }
}
