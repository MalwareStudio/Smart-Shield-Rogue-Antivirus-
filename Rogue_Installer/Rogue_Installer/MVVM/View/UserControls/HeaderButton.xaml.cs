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
using static Rogue_Installer.MVVM.Model.Global;

namespace Rogue_Installer.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for HeaderButton.xaml
    /// </summary>
    public partial class HeaderButton : UserControl
    {
        public HeaderButton()
        {
            InitializeComponent();
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.WindowState = WindowState.Minimized;
            });
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsSetupRunning)
                return;

            Environment.Exit(0);
        }
    }
}
