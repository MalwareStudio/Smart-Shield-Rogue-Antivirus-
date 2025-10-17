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
using Rogue_Installer.MVVM.View.Pages;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Media;
using static Rogue_Installer.MVVM.Model.BitmapGenerator;
using static Rogue_Installer.MVVM.Model.InitializeBitmap;
using static Rogue_Installer.MVVM.Model.Global;
using System.Windows.Threading;

namespace Rogue_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public vmMainWindow _vmMainWindow { get; } = new vmMainWindow();
        public Timer thTimerBack;
        private SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.theme);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vmMainWindow;
            StoreBitmapsIntoMemory();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vmMainWindow.CurrentPage = new MainPage();

            soundPlayer.PlayLooping();
            _vmMainWindow.VolumeIcon = (BitmapImage)FindResource("VolumeUp");

            thTimerBack = new Timer(BackgroundAnimation, null, 0, 10);
        }

        private void BackgroundAnimation(object sender)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _vmMainWindow.RandomBackground = MainBackgroundImages[rand.Next(MainBackgroundImages.Count)];
            });
        }

        private static Random rand = new Random();

        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            var currentVolume = _vmMainWindow.VolumeIcon;
            var volumeUpImage = (BitmapImage)FindResource("VolumeUp");

            if (currentVolume == volumeUpImage)
            {
                soundPlayer.Stop();
                _vmMainWindow.VolumeIcon = (BitmapImage)FindResource("VolumeMuted");
                return;
            }

            soundPlayer.PlayLooping();
            _vmMainWindow.VolumeIcon = volumeUpImage;
        }

        private void MenuControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsSetupRunning)
                _vmMainWindow.IsMenuEnabled = false;
            else
                _vmMainWindow.IsMenuEnabled = true;
        }
    }
}
