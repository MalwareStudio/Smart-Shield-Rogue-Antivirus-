using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using RogueAntivirusPatched.MVVM;
using RogueAntivirusPatched.ViewModel;

namespace RogueAntivirusPatched.View.CustomUserControl
{
    /// <summary>
    /// Interaction logic for MenuItem.xaml
    /// </summary>
    public partial class MenuItem : UserControl
    {
        public vmMenuItem _vmMenuItem { get; set; }

        public static readonly DependencyProperty MenuItemProperty =
            DependencyProperty.Register("MenuItemText", typeof(string), typeof(MenuItem),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, MenuTextChanged));

        public static readonly DependencyProperty SubMenuItemProperty =
            DependencyProperty.Register("SubMenuItemText", typeof(string), typeof(MenuItem),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SubMenuTextChanged));

        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register("FontColor", typeof(Brush), typeof(MenuItem),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, FontColorChanged));

        public static readonly DependencyProperty SubFontColorProperty =
            DependencyProperty.Register("SubFontColor", typeof(Brush), typeof(MenuItem),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SubFontColorChanged));
        public static readonly DependencyProperty MenuItemImageProperty =
    DependencyProperty.Register("MenuItemImage", typeof(BitmapImage), typeof(MenuItem),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, MenuItemImageChanged));
        public MenuItem()
        {
            InitializeComponent();
            _vmMenuItem = new vmMenuItem();
            DataContext = _vmMenuItem;
        }

        public string MenuItemText
        {
            get { return (string)GetValue(MenuItemProperty); }
            set { SetValue(MenuItemProperty, value); }
        }

        public string SubMenuItemText
        {
            get { return (string)GetValue(SubMenuItemProperty); }
            set { SetValue(SubMenuItemProperty, value); }
        }

        public Brush FontColor
        {
            get { return (Brush)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        public Brush SubFontColor
        {
            get { return (Brush)GetValue(SubFontColorProperty); }
            set { SetValue(SubFontColorProperty, value); }
        }

        public BitmapImage MenuItemImage
        {
            get { return (BitmapImage)GetValue(MenuItemImageProperty); }
            set { SetValue(MenuItemImageProperty, value); }
        }

        public static void MenuTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuItem control && control._vmMenuItem != null)
            {
                control._vmMenuItem.MenuItemText = e.NewValue as string;
            }
        }

        public static void SubMenuTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuItem control && control._vmMenuItem != null)
            {
                control._vmMenuItem.SubMenuItemText = e.NewValue as string;
            }
        }

        public static void FontColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuItem control && control._vmMenuItem != null)
            {
                control._vmMenuItem.FontColor = e.NewValue as Brush;
            }
        }

        public static void SubFontColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuItem control && control._vmMenuItem != null)
            {
                control._vmMenuItem.SubFontColor = e.NewValue as Brush;
            }
        }

        public static void MenuItemImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuItem control && control._vmMenuItem != null)
            {
                control._vmMenuItem.MenuItemImage = e.NewValue as BitmapImage;
            }
        }
    }
}
