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

namespace RogueAntivirusPatched.View.CustomUserControl
{
    /// <summary>
    /// Interaction logic for CustomRadialButton.xaml
    /// </summary>
    public partial class CustomRadialButton : UserControl
    {
        public static readonly DependencyProperty CustomContentProperty =
            DependencyProperty.Register("SetContent", typeof(string), typeof(CustomRadialButton),
                new PropertyMetadata("Hello"));

        public static readonly DependencyProperty CustomIsEnabledProperty =
            DependencyProperty.Register("SetIsEnabled", typeof(bool), typeof(CustomRadialButton),
                new PropertyMetadata(true));

        public static readonly DependencyProperty CustomIsCheckedProperty =
            DependencyProperty.Register("SetIsChecked", typeof(bool), typeof(CustomRadialButton),
                new PropertyMetadata(false));

        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("SetGroupName", typeof(string), typeof(CustomRadialButton),
                new PropertyMetadata(""));

        public string SetContent
        {
            get => (string)GetValue(ContentProperty); 
            set => SetValue(ContentProperty, value);
        }

        public bool SetIsEnabled
        {
            get => (bool)GetValue(CustomIsEnabledProperty);
            set => SetValue(CustomIsEnabledProperty, value);
        }

        public bool SetIsChecked
        {
            get => (bool)GetValue(CustomIsCheckedProperty);
            set => SetValue(CustomIsCheckedProperty, value);
        }

        public string SetGroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public CustomRadialButton()
        {
            InitializeComponent();
        }
    }
}
