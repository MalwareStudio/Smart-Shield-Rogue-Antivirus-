using Rogue_Installer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using static Rogue_Installer.MVVM.Model.BitmapGenerator;
using static Rogue_Installer.MVVM.Model.InitializeBitmap;

namespace Rogue_Installer.MVVM.View.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public vmAboutPage _vmAboutPage;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private Timer thBackgroundAnim;
        private static Random rand = new Random();
        public AboutPage()
        {
            InitializeComponent();
            _vmAboutPage = new vmAboutPage();
            DataContext = _vmAboutPage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.PageTitle = mainWindow._vmMainWindow.AboutContent;

            thBackgroundAnim = new Timer(BackgroundAnimation, null, 0, 10);
        }

        private void BackgroundAnimation(object sender)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _vmAboutPage.AnimBackground = AboutBackgroundImages[rand.Next(AboutBackgroundImages.Count)];
            });
        }
    }
}
