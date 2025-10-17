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
using System.Threading;
using System.IO;
using static Rogue_Installer.MVVM.Model.BitmapGenerator;
using static Rogue_Installer.MVVM.Model.InitializeBitmap;

namespace Rogue_Installer.MVVM.View.Pages
{
    /// <summary>
    /// Interaction logic for Credits.xaml
    /// </summary>
    public partial class Credits : Page
    {
        private vmCreditsPage _vmCreditsPage;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private Timer thBackgroundAnim;
        private static Random rand = new Random();
        public Credits()
        {
            InitializeComponent();
            _vmCreditsPage = new vmCreditsPage();
            DataContext = _vmCreditsPage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.PageTitle = mainWindow._vmMainWindow.CreditsContent;
            thBackgroundAnim = new Timer(BackgroundAnimation, null, 0, 10);
        }

        private void BackgroundAnimation(object sender)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _vmCreditsPage.BackgroundCyberSoldier = CyberBackgroundImages[rand.Next(CyberBackgroundImages.Count)];
                _vmCreditsPage.BackgroundExlon = ExlonBackgroundImages[rand.Next(ExlonBackgroundImages.Count)];
            });
        }
    }
}
