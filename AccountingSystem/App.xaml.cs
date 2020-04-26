using AccountingSystem.Utilities;
using AccountingSystem.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AccountingSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            MainWindow = mainWindow;
            LoginWindow win = new LoginWindow(); 

            if (win.ShowDialog() == true)
            {
                mainWindow.SetUser(win.ViewModel.LoggedUser);
                mainWindow.SetMainPage(MainPageFabric.CreatePage(win.ViewModel.LoggedUser));
                MainWindow.Show();
            }
            else
            {
                MainWindow.Close();
            }
          
        }
    
    }
}
