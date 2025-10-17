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
using static RogueAntivirusPatched.Global.UISettings;
using System.Runtime.CompilerServices;

namespace RogueAntivirusPatched.Advertisement
{
    internal class NotifyAd
    {
        private ToastContentBuilder toast;

        public NotifyAd()
        {
            ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;
        }

        private static bool Ignore = false;

        public class Arguments
        {
            public const string Upgrade = "UpgradeBtn";
            public const string Antivirus = "AntivirusBtn";
            public const string Registry = "RegistryBtn";
            public const string Cleaner = "CleanerBtn";
            public const string Contact = "ContactBtn";
            public const string Exception = "ShowOnlyWindow";
        }

        public void SetNotification(string text, string header, string btnContentYes,
            string btnContentNo, string BtnOkArgument, string specialArgs = "default", 
            ToastDuration duration = ToastDuration.Long, bool ignore = false)
        {
            toast = new ToastContentBuilder();

            Ignore = ignore;
            toast.AddText(text);
            toast.AddHeader("5892", header, specialArgs);
            toast.AddButton(btnContentYes, ToastActivationType.Foreground, BtnOkArgument);
            toast.AddButton(btnContentNo, ToastActivationType.Foreground, "LaterBtnArgs");
            toast.SetToastDuration(duration);

            toast.Show();
        }
        public void SetNotification(string text, string header, string btnContentYes, 
            string btnContentNo, string BtnOkArgument,
            Uri image, string specialArgs = "default")
        {
            toast = new ToastContentBuilder();

            toast.AddText(text);
            toast.AddHeader("5892", header, specialArgs);
            toast.AddButton(btnContentYes, ToastActivationType.Foreground, BtnOkArgument);
            toast.AddButton(btnContentNo, ToastActivationType.Foreground, "NoBtnArgs");
            toast.AddAppLogoOverride(image);
            toast.SetToastDuration(ToastDuration.Long);

            toast.Show();
        }

        public void SetNotification(string text, string header, string specialArgs = "default")
        {
            toast = new ToastContentBuilder();

            toast.AddText(text);
            toast.AddHeader("5892", header, specialArgs);
            toast.SetToastDuration(ToastDuration.Short);

            toast.Show();
        }

        private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            if (e is ToastNotificationActivatedEventArgsCompat thisToast)
            {
                var toastArgs = ToastArguments.Parse(thisToast.Argument);

                string strToastArgs = toastArgs.ToString();

                if (Ignore)
                    return;

                if (string.IsNullOrWhiteSpace(strToastArgs))
                {
                    ShowWindow();
                    return;
                }

                if (!Variables.IsPageWorking)
                {
                    if (string.IsNullOrWhiteSpace(strToastArgs))
                    {
                        ShowWindow();
                        return;
                    }
                     
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
                        });
                    });
                }
                else
                    Messages.ProcessIsRunning();
            }

            ToastNotificationManagerCompat.OnActivated -= ToastNotificationManagerCompat_OnActivated;
        }

        private void CanRedirectToRegistration(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != Arguments.Upgrade)
                return;

            ShowWindow();
            mainWindow.VmMainWindow.ContentPage = new RegisterPage();
        }

        private void CanRedirectToAntivirus(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != Arguments.Antivirus)
                return;

            ShowWindow();
            mainWindow.VmMainWindow.ContentPage = new AntivirusPage();
        }

        private void CanRedirectToRegistry(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != Arguments.Registry)
                return;

            ShowWindow();
            mainWindow.VmMainWindow.ContentPage = new RegistryPage();
        }
        private void CanRedirectToCleaner(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != Arguments.Cleaner)
                return;

            ShowWindow();
            mainWindow.VmMainWindow.ContentPage = new JunkCleanerPage();
        }

        private void CanRedirectToHelp(MainWindow mainWindow, string strToastArgs)
        {
            if (strToastArgs != Arguments.Contact)
                return;

            ShowWindow();
            Contact.GetToEmail();
        }
    }
}
