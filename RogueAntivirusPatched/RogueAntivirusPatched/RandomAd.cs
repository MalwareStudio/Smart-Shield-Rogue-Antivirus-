using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.View.Pages;

namespace RogueAntivirusPatched.Advertisement
{
    internal class RandomAd
    {
        private DispatcherTimer timerWindowAd, timerNotifyAd;
        private Random rand;
        private KeySender keySender;
        private NotifyAd notifyAd;

        public RandomAd()
        {
            keySender = new KeySender();
            notifyAd = new NotifyAd();
        }

        public void SetUpAds()
        {
            timerWindowAd = new DispatcherTimer();
            timerWindowAd.Interval = TimeSpan.FromSeconds(5);
            timerWindowAd.IsEnabled = true;
            timerWindowAd.Tick += TimerWindowAd_Tick;

            timerNotifyAd = new DispatcherTimer();
            timerNotifyAd.Interval = TimeSpan.FromSeconds(180);
            timerNotifyAd.IsEnabled = false;
            timerNotifyAd.Tick += TimerNotifyAd_Tick;
        }

        private void TimerNotifyAd_Tick(object sender, EventArgs e)
        {
            CheckLicense();

            rand = new Random();

            int index = rand.Next(5);

            string text, header, btnYes, btnNo, imageSource, btnYesArgument = "";

            switch (index)
            {
                case 0:
                    text = "It looks like you are running the antivirus in a Trial Mode. " + Environment.NewLine + "We can provide you better features and make your PC safer if you upgrade to the Pro Version. " + Environment.NewLine + "Click on this notification and we will redirect you to the Registration Page.";
                    header = "Upgrade to the Pro Version";
                    btnYes = "Upgrade to Pro";
                    btnNo = "Not now";
                    btnYesArgument = "UpgradeBtn";

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument);

                    break;
                case 1:
                    text = "Your pc might be in a danger! " + Environment.NewLine + "Is it very important to scan your pc at least once in a week. Keep in mind that some malwares can steal your precious data. To prevent such a dissaster, scan your pc immediatelly!";
                    header = "Scan your PC";
                    btnYes = "Okay";
                    btnNo = "Maybe Later";
                    btnYesArgument = "AntivirusBtn";
                    imageSource = "C:\\Users\\jena0\\source\\repos\\RogueAntivirusPatched\\RogueAntivirusPatched\\bin\\Debug\\encrypted.png";

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
                case 2:
                    text = "Did you know that most of applications create registry data which helps them to strore some of their settings? However, some registry data can be left unused and application which created them might not get erase them. " + Environment.NewLine + "Search for these useless data and get better performence!";
                    header = "Keep your Registry Clean";
                    btnYes = "Okay";
                    btnNo = "Maybe Later";
                    btnYesArgument = "RegistryBtn";
                    imageSource = "C:\\Users\\jena0\\source\\repos\\RogueAntivirusPatched\\RogueAntivirusPatched\\bin\\Debug\\encrypted.png";

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
                case 3:
                    text = @"When applications are running, sometimes they produce ""Log files"" which are usually filled with bunch of error lines. " + Environment.NewLine + "Deleting these files can release up to 10 GB of space! " + Environment.NewLine + "Do you want to check how much space can you get?";
                    header = "Increase the Disk Space";
                    btnYes = "Okay";
                    btnNo = "Maybe Later";
                    btnYesArgument = "CleanerBtn";
                    imageSource = "C:\\Users\\jena0\\source\\repos\\RogueAntivirusPatched\\RogueAntivirusPatched\\bin\\Debug\\encrypted.png";

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
                case 4:
                    text = @"In case of complex probles we are online 24/7 to solve them." + Environment.NewLine + "Don't hessitate to contact us via our email!";
                    header = "Do you need a Special Help?";
                    btnYes = "Contact Us!";
                    btnNo = "Maybe Later";
                    btnYesArgument = "ContactBtn";
                    imageSource = "C:\\Users\\jena0\\source\\repos\\RogueAntivirusPatched\\RogueAntivirusPatched\\bin\\Debug\\encrypted.png";

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
            }
        }

        private event EventHandler eventHandlerYes;

        private void TimerWindowAd_Tick(object sender, EventArgs e)
        {
            CheckLicense();

            bool wait = true;
            string text = "Upgrade to Pro Version";
            string bodyText = "It looks like you are still in the Trial Mode! " + Environment.NewLine + "Keep in mind that the Trial Mode is very limited and it will expired after 7 days. When the Trial Mode ends, you will lose access to all of the features and caught Threats will be released from the quarantine." + Environment.NewLine + Environment.NewLine +
                "Don't hesitate to Upgrade to the Pro so you can keep your PC safe!";
            string yesBtn = "Buy Now";
            string noBtn = "Maybe later";
            string title = "Special Offer";
            double width = 700.0;
            double height = 500.0;
            BitmapImage bitmapImage = new BitmapImage(new Uri("C:\\Users\\jena0\\source\\repos\\RogueAntivirusPatched\\RogueAntivirusPatched\\bin\\Debug\\ad image.png"));

            eventHandlerYes += RandomAd_eventHandlerYes;

            Windows.Advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, eventHandlerYes, null, width, height, bitmapImage);
        }

        private void RandomAd_eventHandlerYes(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.VmMainWindow.ContentPage = new RegisterPage();
                });
            });

            eventHandlerYes -= RandomAd_eventHandlerYes;
        }

        public void OnlyForPro()
        {
            CheckLicense();

            bool wait = true;
            string text = "Why it doesn't work?";
            string bodyText = "Feature which you are trying to launch is only available in Pro Version. If you want to get access to all of the features without any restrictions, Upgrade to the Pro Version Now!";
            string yesBtn = "Upgrade Now";
            string noBtn = "Maybe Later";
            string title = "This Feature Is Not Available";
            double width = 700.0;
            double height = 400.0;
            //BitmapImage bitmapImage = new BitmapImage(new Uri("C:\\Users\\jena0\\source\\repos\\RogueAntivirusPatched\\RogueAntivirusPatched\\bin\\Debug\\blue sad.png"));

            eventHandlerYes += OnlyForPro_eventHandlerYes;

            Windows.Advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, eventHandlerYes, null, width, height, null);
        }

        private void OnlyForPro_eventHandlerYes(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.VmMainWindow.ContentPage = new RegisterPage();
                });
            });

            eventHandlerYes -= OnlyForPro_eventHandlerYes;
        }

        private void CheckLicense()
        {
            if (keySender.HasLicense)
            {
                timerWindowAd.IsEnabled = false;
                timerWindowAd.Stop();
                timerNotifyAd.IsEnabled = false;
                timerNotifyAd.Stop();
            }
        }
    }
}
