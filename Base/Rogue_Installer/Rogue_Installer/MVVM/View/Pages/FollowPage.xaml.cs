using Rogue_Installer.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
    /// Interaction logic for FollowPage.xaml
    /// </summary>
    public partial class FollowPage : Page
    {
        private vmFollowUsPage _vmFollowUsPage;
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public FollowPage()
        {
            InitializeComponent();
            _vmFollowUsPage = new vmFollowUsPage();
            DataContext = _vmFollowUsPage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow._vmMainWindow.PageTitle = mainWindow._vmMainWindow.FollowContent;
        }

        private void YouTube_Click(object sender, MouseButtonEventArgs e)
        {
            string[] channels =
            {
                "https://www.youtube.com/@exlon",
                "https://www.youtube.com/@ClutterTech"
            };

            foreach (string channel in channels)
            {
                OpenUrl(channel);
            }
        }

        private void Discord_Click(object sender, MouseButtonEventArgs e)
        {
            OpenUrl("https://discord.gg/MKmNF57Re4");
        }

        private void Github_Click(object sender, MouseButtonEventArgs e)
        {
            OpenUrl("https://github.com/MalwareStudio");
        }

        private void OpenUrl(string url)
        {
            using (var process = new Process())
            {
                var processInfo = new ProcessStartInfo();
                processInfo.FileName = url;
                processInfo.UseShellExecute = true;

                process.StartInfo = processInfo;
                process.Start();
            }
        }
    }
}
