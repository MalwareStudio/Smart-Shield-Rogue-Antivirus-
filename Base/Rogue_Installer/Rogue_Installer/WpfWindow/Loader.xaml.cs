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
using System.Windows.Shapes;
using Rogue_Installer.MVVM.ViewModel;
using System.Windows.Media.Animation;
using static Rogue_Installer.MVVM.Model.BitmapGenerator;
using static Rogue_Installer.MVVM.Model.Global;

namespace Rogue_Installer.WpfWindow
{
    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    public partial class Loader : Window
    {
        public vmLoader sharedVmLoader { get; } = new vmLoader();
        public Loader()
        {
            InitializeComponent();
            sharedVmLoader = new vmLoader();
            DataContext = sharedVmLoader;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation
            {
                From = 0, 
                To = 1.0,
                Duration = TimeSpan.FromSeconds(3)
            };

            MainGrid.BeginAnimation(Grid.OpacityProperty, doubleAnimation);

            Task.Run(() =>
            {
                BackgroundTask();
                Application.Current.Dispatcher.Invoke(() => { sharedVmLoader.ProgressDescription = "Everything is ready, Let's GO!!!"; });
                Task.Delay(2000);
                ProcessCompleted();
            });
        }

        private void BackgroundTask()
        {
            SetUp.Generate();
        }

        private void ProcessCompleted()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(3)
                };

                doubleAnimation.Completed += DoubleAnimation_Completed;

                MainGrid.BeginAnimation(Grid.OpacityProperty, doubleAnimation);
            });
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            var mainWindow = new MainWindow();

            Application.Current.MainWindow = mainWindow;

            mainWindow.Show();
            
            Close();
        }
    }
}
