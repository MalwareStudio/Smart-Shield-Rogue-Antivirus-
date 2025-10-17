using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
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
using Rogue_Installer.MVVM.View.Pages;
using static Rogue_Installer.MVVM.Model.Global;

namespace Rogue_Installer.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for InstallExitButton.xaml
    /// </summary>
    public partial class InstallExitButton : UserControl
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public Button ExitButton => ExitBtn;
        public Button InstallButton => InstallBtn;
        public InstallExitButton()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);
        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            string pageTitle = mainWindow._vmMainWindow.PageTitle;
            string menuHeader = mainWindow._vmMainWindow.LastWarnContent;

            if (pageTitle == menuHeader)
            {
                mainWindow._vmMainWindow.CurrentPage = new InstallerPage();
                mainWindow._vmMainWindow.PageTitle = "Setup";
                return;
            }

            mainWindow._vmMainWindow.CurrentPage = new LastWarnPage();
        }
    }
}
