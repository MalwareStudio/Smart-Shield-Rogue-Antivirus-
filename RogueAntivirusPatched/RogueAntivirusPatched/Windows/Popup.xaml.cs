using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Threading;
using RogueAntivirusPatched.Classes;
using System.Windows.Controls.Primitives;
using RogueAntivirusPatched.ViewModel;
using Application = System.Windows.Application;
using RogueAntivirusPatched.View.Pages;
using System.Media;
using Windows.Management;
using System.IO;
using RogueAntivirusPatched.Global;
using static RogueAntivirusPatched.Global.UISettings;
using static RogueAntivirusPatched.Global.Convertor;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Input;

namespace RogueAntivirusPatched.Windows
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        private static vmPopup _vmPopup;
        private Sounds sounds;

        private event EventHandler popupButton;

        [Flags]
        public enum PopUpDuration
        {
            ANIM_SHORT = 0,
            ANIM_LONG = 1
        }

        private static PopUpDuration popUpDuration = PopUpDuration.ANIM_SHORT;

        public Popup()
        {
            InitializeComponent();
            _vmPopup = new vmPopup();
            DataContext = _vmPopup;
            sounds = new Sounds();
        }

        [Flags]
        enum AnimateWindowFlags
        {
            AW_HOR_POSITIVE = 0x0000000,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }

        [DllImport("user32.dll")]
        static extern bool AnimateWindow(IntPtr hWnd, int time, AnimateWindowFlags flags);

        public void ShowPopup(string title = "", string subTitle = "", string text = "", string contentButton = "Click", Bitmap imageLocation = null, PopUpDuration duration = PopUpDuration.ANIM_SHORT,
            EventHandler buttonHandler = null)
        {
            popUpDuration = duration;

            _vmPopup.Title = title;
            _vmPopup.SubTitle = subTitle;
            _vmPopup.Text = text;
            _vmPopup.ContentButton = contentButton;

            popupButton = buttonHandler;

            if (imageLocation != null)
                _vmPopup.Picture = ToBitmapImagePNG(imageLocation);
            NotificationHandler.popups.Add(this);
            Show();
        }

        private void Animate(bool appear)
        {
            WindowTranslate.X = appear ? this.Width : 0;
            var slide_anim = new DoubleAnimation
            {
                From = WindowTranslate.X,
                To = appear ? 0 : this.Width,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            WindowTranslate.BeginAnimation(TranslateTransform.XProperty, slide_anim);
        }

        private async void Window_SourceInitialized(object sender, EventArgs e)
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            this.Left = bounds.Right - bounds.Left - this.Width;
            this.Top = bounds.Bottom - bounds.Top - this.Height * NotificationHandler.popups.Count;

            Animate(true);
            if (popUpDuration == PopUpDuration.ANIM_SHORT)
                await Task.Delay(3000);
            else
                await Task.Delay(8000);
            this.Close();
        }

        private bool can_close = false;

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !can_close;
            if (can_close) return;
            can_close = true;
            Animate(false);
            await Task.Delay(700);
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NotificationHandler.popups.Remove(this);
            NotificationHandler.UpdatePositions();
        }

        private void BtnRemove_Click(object sender, MouseButtonEventArgs e)
        {
            popupButton?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
