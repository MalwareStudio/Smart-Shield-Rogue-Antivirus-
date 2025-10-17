using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rogue_Installer.MVVM.ViewModel;
using Rogue_Installer.MVVM.View.Pages;

namespace Rogue_Installer.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        private MainWindow mainWindow;
        public MenuControl()
        {
            InitializeComponent();
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.CurrentPage = new MainPage();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.CurrentPage = new AboutPage();
        }

        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.CurrentPage = new Credits();
        }

        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.CurrentPage = new FollowPage();
        }

        private void MenuControlName_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }
    }
}
