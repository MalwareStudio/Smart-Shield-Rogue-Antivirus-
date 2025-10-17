using Rogue_Installer.MVVM.ViewModel;
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

namespace Rogue_Installer.MVVM.View.Pages
{
    /// <summary>
    /// Interaction logic for LastWarnPage.xaml
    /// </summary>
    public partial class LastWarnPage : Page
    {
        private vmMainWindow _vmMainWindow;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public LastWarnPage()
        {
            InitializeComponent();
            _vmMainWindow = new vmMainWindow();
            DataContext = _vmMainWindow;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.PageTitle = mainWindow._vmMainWindow.LastWarnContent;
        }
    }
}
