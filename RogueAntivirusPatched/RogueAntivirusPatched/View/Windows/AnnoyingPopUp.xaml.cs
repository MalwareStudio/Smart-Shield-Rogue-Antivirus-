using RogueAntivirusPatched.Classes;
using RogueAntivirusPatched.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using form = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static RogueAntivirusPatched.Global.Convertor;
using System.Windows.Threading;

namespace RogueAntivirusPatched.View.Windows
{
    /// <summary>
    /// Interaction logic for AnnoyingPopUp.xaml
    /// </summary>
    public partial class AnnoyingPopUp : Window
    {
        private static vmAnnoyingPopUp _vmAnnoyingPopUp;
        private static readonly Random rand = new Random();

        public AnnoyingPopUp()
        {
            InitializeComponent();
            _vmAnnoyingPopUp = new vmAnnoyingPopUp();
            DataContext = _vmAnnoyingPopUp;
        }

        public EventHandler button1Handler, button2Handler;
        private static object AnimStyle;
        private static UnmanagedMemoryStream SoundEffect;
        private static SoundPlayer soundPlayer;

        public void ShowAnnoyingPopUp(string title, string subTitle, string text, 
            Bitmap bitmap, Thickness bitmapMargin = new Thickness(), int width = 500, int height = 400, UnmanagedMemoryStream soundEffect = null, string contentButton1 = "Yes", string contentButton2 = "No",
            EventHandler btnFirst = null, EventHandler btnSecond = null,
            AnimationTypes animationType = AnimationTypes.SwipeFromLeft, 
            int swipeDuration = 500, string bitmapAnimResource = "SwingAnimation", bool autoClick = false)
        {
            _vmAnnoyingPopUp.Title = title;
            _vmAnnoyingPopUp.SubTitle = subTitle;
            _vmAnnoyingPopUp.Text = text;
            _vmAnnoyingPopUp.PopImage = ToBitmapImagePNG(bitmap);
            _vmAnnoyingPopUp.ContentButtonFirst = contentButton1;
            _vmAnnoyingPopUp.ContentButtonSecond = contentButton2;
            _vmAnnoyingPopUp.ImageMargin = bitmapMargin;
            _vmAnnoyingPopUp.GridWidth = width;
            _vmAnnoyingPopUp.GridHeight = height;

            currentAnimation = animationType;
            animationDuration = swipeDuration;          

            button1Handler = btnFirst;
            button2Handler = btnSecond;
            AnimStyle = FindResource(bitmapAnimResource);

            SoundEffect = soundEffect;

            Show();

            if (autoClick)
            {
                var timerAutoClick = new DispatcherTimer();
                timerAutoClick.IsEnabled = false;
                timerAutoClick.Interval = TimeSpan.FromSeconds(rand.Next(5, 15));
                timerAutoClick.Tick += (s, e) =>
                {
                    BtnFirstOption_Click(this, new RoutedEventArgs());
                    timerAutoClick.Stop();
                };
                timerAutoClick.Start();
            }
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Animate(true, currentAnimation, animationDuration);
        }

        public enum AnimationTypes : int
        {
            SwipeFromLeft = 0,
            SwipeFromRight = 1,
            SwipeFromTop = 2,
            SwipeFromBottom = 3,
        }

        private static AnimationTypes currentAnimation = AnimationTypes.SwipeFromLeft;
        private static int animationDuration = 800;

        private async void Animate(bool appear, AnimationTypes animType, int duration)
        {
            currentAnimation = animType;
            animationDuration = duration;

            var centerScreen = (form.Screen.PrimaryScreen.Bounds.Width / 2) - (Width / 2);
            var centerYScreen = (form.Screen.PrimaryScreen.Bounds.Height / 2) - (Height / 2);
            var endScreen = form.Screen.PrimaryScreen.Bounds.Width;
            var endYScreen = form.Screen.PrimaryScreen.Bounds.Height;
            DoubleAnimation swipeAnim;

            switch (animType)
            {
                case AnimationTypes.SwipeFromLeft:

                    swipeAnim = new DoubleAnimation
                    {
                        From = appear ? -Width : 0,
                        To = appear ? 0 : Width,
                        Duration = TimeSpan.FromMilliseconds(duration),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    ContentTransform.BeginAnimation(TranslateTransform.XProperty, swipeAnim);

                    break;
                case AnimationTypes.SwipeFromRight:

                    swipeAnim = new DoubleAnimation
                    {
                        From = appear ? Width : 0,
                        To = appear ? 0 : -Width,
                        Duration = TimeSpan.FromMilliseconds(duration),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    ContentTransform.BeginAnimation(TranslateTransform.XProperty, swipeAnim);

                    break;
                case AnimationTypes.SwipeFromTop:

                    swipeAnim = new DoubleAnimation
                    {
                        From = appear ? -Height : 0,
                        To = appear ? 0 : Height,
                        Duration = TimeSpan.FromMilliseconds(duration),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    ContentTransform.BeginAnimation(TranslateTransform.YProperty, swipeAnim);

                    break;
                case AnimationTypes.SwipeFromBottom:

                    swipeAnim = new DoubleAnimation
                    {
                        From = appear ? Height : 0,
                        To = appear ? 0 : -Height,
                        Duration = TimeSpan.FromMilliseconds(duration),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    ContentTransform.BeginAnimation(TranslateTransform.YProperty, swipeAnim);

                    break;
            }

            await Task.Delay(animationDuration);

            if (SoundEffect != null)
            {
                using (var mem = new MemoryStream())
                {
                    SoundEffect.Position = 0;
                    SoundEffect.CopyTo(mem);
                    mem.Position = 0;

                    using (soundPlayer = new SoundPlayer(mem))
                    {
                        soundPlayer.Play();
                        soundPlayer.Disposed += Sound_Disposed;
                    }
                }
            }

            ThisImage.Style = (Style)AnimStyle;
        }

        private void Sound_Disposed(object sender, EventArgs e)
        {
            SoundEffect = null;
            soundPlayer.Disposed -= Sound_Disposed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void BtnFirstOption_Click(object sender, RoutedEventArgs e)
        {
            Animate(false, currentAnimation, animationDuration);

            await Task.Delay(animationDuration);

            button1Handler?.Invoke(this, EventArgs.Empty);

            Close();
        }

        private async void BtnSecondOption_Click(object sender, RoutedEventArgs e)
        {
            Animate(false, currentAnimation, animationDuration);

            await Task.Delay(animationDuration);

            button2Handler?.Invoke(this, EventArgs.Empty);

            Close();
        }
    }
}
