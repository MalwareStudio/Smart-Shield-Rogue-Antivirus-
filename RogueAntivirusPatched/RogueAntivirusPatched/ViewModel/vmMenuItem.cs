using RogueAntivirusPatched.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static RogueAntivirusPatched.Global.Convertor;

namespace RogueAntivirusPatched.ViewModel
{
    public class vmMenuItem : VmBase
    {
        private string menuItemText;
        public string MenuItemText
        {
            get { return menuItemText; }
            set
            {
                menuItemText = value;
                PropertyChnaged();
            }
        }

        private Brush fontColor;

        public Brush FontColor
        {
            get { return fontColor; }
            set
            {
                fontColor = value;
                PropertyChnaged();
            }
        }

        private string subMenuItemText;
        public string SubMenuItemText
        {
            get { return subMenuItemText; }
            set
            {
                subMenuItemText = value;
                PropertyChnaged();
            }
        }

        private Brush subFontColor;

        public Brush SubFontColor
        {
            get { return subFontColor; }
            set
            {
                subFontColor = value;
                PropertyChnaged();
            }
        }

        private BitmapImage menuItemImage = ToBitmapImagePNG(Properties.Resources.sweep);

        public BitmapImage MenuItemImage
        {
            get { return menuItemImage; }
            set 
            { 
                menuItemImage = value; 
                PropertyChnaged();
            }
        }

    }
}
