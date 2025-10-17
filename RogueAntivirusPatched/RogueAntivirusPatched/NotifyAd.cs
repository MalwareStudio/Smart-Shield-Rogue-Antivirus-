using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using Windows;
using System.Windows;
using RogueAntivirusPatched.View.Pages;
using RogueAntivirusPatched.ViewModel;
using System.Diagnostics;
using RogueAntivirusPatched.Global;

namespace RogueAntivirusPatched.Advertisement
{
    internal class NotifyAd
    {
        private ToastContentBuilder toast;

        public void SetNotification(string text, string header, string btnContentYes,
            string btnContentNo, string BtnOkArgument)
        {
            toast = new ToastContentBuilder();

            toast.AddText(text);
            toast.AddHeader("5892", header, "idk");
            toast.AddButton(btnContentYes, ToastActivationType.Foreground, BtnOkArgument);
            toast.AddButton(btnContentNo, ToastActivationType.Foreground, "LaterBtnArgs");
            toast.SetToastDuration(ToastDuration.Long);

            ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;

            toast.Show();
        }
        public void SetNotification(string text, string header, string btnContentYes, 
            string btnContentNo, string BtnOkArgument,
            string ImagePath = "C:\\Users\\jena0\\source\\repos\\RogueAntivirusPatched\\RogueAntivirusPatched\\bin\\Debug\\encrypted.png")
        {
            toast = new ToastContentBuilder();

            var image = new Uri(ImagePath);

            toast.AddText(text);
            toast.AddHeader("5892", header, "ArgumentAntivirus");
            toast.AddButton(btnContentYes, ToastActivationType.Foreground, BtnOkArgument);
            toast.AddButton(btnContentNo, ToastActivationType.Foreground, "NoBtnArgs");
            toast.AddAppLogoOverride(image);
            toast.SetToastDuration(ToastDuration.Long);

            ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;

            toast.Show();
        }

        private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            if (e is ToastNotificationActivatedEventArgsCompat thisToast)
            {
                ToastArguments toastArgs = ToastArguments.Parse(thisToast.Argument);

                string strToastArgs = toastArgs.ToString();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.Dispatcher.Invoke(() =>
                    {
                        CanRedirectToRegistration(mainWindow, strToastArgs);
                        CanRedirectToAntivirus(mainWindow, strToastArgs);
                        CanRedirectToRegistry(mainWindow, strToastArgs);
                        CanRedirectToCleaner(mainWindow, strToastArgs);
                        CanRedirectToHelp(mainWindow, strToastArgs);
                        mainWindow.WindowState = WindowState.Normal;
                        mainWindow.Activate();
                    });
                });
            }

            ToastNotificationManagerCompat.OnActivated -= ToastNotificationManagerCompat_OnActivated;
        }

        private void CanRedirectToRegistration(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != "UpgradeBtn")
                return;

            mainWindow.VmMainWindow.ContentPage = new RegisterPage();
        }

        private void CanRedirectToAntivirus(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != "AntivirusBtn")
                return;

            mainWindow.VmMainWindow.ContentPage = new AntivirusPage();
        }

        private void CanRedirectToRegistry(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != "RegistryBtn")
                return;

            mainWindow.VmMainWindow.ContentPage = new RegistryPage();
        }
        private void CanRedirectToCleaner(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != "CleanerBtn")
                return;

            mainWindow.VmMainWindow.ContentPage = new JunkCleanerPage();
        }

        private void CanRedirectToHelp(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != "ContactBtn")
                return;

            Contact.GetToEmail();
        }
    }
}
