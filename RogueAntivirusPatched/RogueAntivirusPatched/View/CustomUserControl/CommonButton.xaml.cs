using RogueAntivirusPatched.ViewModel;
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
using System.Windows.Shapes;
using Windows.UI.Xaml.Input;

namespace RogueAntivirusPatched.View.CustomUserControl
{
    /// <summary>
    /// Interaction logic for CommonButton.xaml
    /// </summary>
    public partial class CommonButton : UserControl
    {
        public static readonly DependencyProperty ButtonFontSizeProperty =
            DependencyProperty.Register(nameof(SetFontSize), typeof(double), typeof(CommonButton),
                new PropertyMetadata(14.0));

        public static readonly DependencyProperty ButtonContentProperty =
    DependencyProperty.Register(nameof(SetContent), typeof(string), typeof(CommonButton),
        new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ButtonWidthProperty =
    DependencyProperty.Register(nameof(SetWidth), typeof(double), typeof(CommonButton),
        new PropertyMetadata(100.0));

        public static readonly DependencyProperty ButtonHeightProperty =
    DependencyProperty.Register(nameof(SetHeight), typeof(double), typeof(CommonButton),
        new PropertyMetadata(30.0));

        public static readonly DependencyProperty ButtonIsEnabledProperty =
    DependencyProperty.Register(nameof(SetIsEnabled), typeof(bool), typeof(CommonButton),
        new PropertyMetadata(true));

        public double SetFontSize
        {
            get => (double)GetValue(ButtonFontSizeProperty);
            set => SetValue(ButtonFontSizeProperty, value);
        }

        public string SetContent
        {
            get => (string)GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }

        public double SetWidth
        {
            get => (double)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

        public double SetHeight
        {
            get => (double)GetValue(ButtonHeightProperty);
            set => SetValue(ButtonHeightProperty, value);
        }

        public bool SetIsEnabled
        {
            get => (bool)GetValue(ButtonIsEnabledProperty);
            set => SetValue(ButtonIsEnabledProperty, value);
        }

        public CommonButton()
        {
            InitializeComponent();
        }
    }
}
