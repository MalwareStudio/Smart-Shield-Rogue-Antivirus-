using RogueAntivirusPatched.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using static RogueAntivirusPatched.Global.Convertor;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmKeystrokeTrigger : VmBase
    {
		private int shieldWidth = 350;

        public int ShieldWidth
        {
			get { return shieldWidth; }
			set 
			{ 
				shieldWidth = value;
				PropertyChnaged();
			}
		}

        private int shieldHeight = 350;

        public int ShieldHeight
        {
            get { return shieldHeight; }
            set
            {
                shieldHeight = value;
                PropertyChnaged();
            }
        }

        private BitmapImage backgroundImage;

        public BitmapImage BackgroundImage
        {
            get { return backgroundImage; }
            set 
            { 
                backgroundImage = value; 
                PropertyChnaged();
            }
        }

        private BitmapImage shieldImage = ToBitmapImagePNG(Properties.Resources.shield_cursedB);

        public BitmapImage ShieldImage
        {
            get { return shieldImage; }
            set
            {
                shieldImage = value;
                PropertyChnaged();
            }
        }

        private Visibility isShieldVisible = Visibility.Hidden;

        public Visibility IsShieldVisible
        {
            get { return isShieldVisible; }
            set 
            { 
                isShieldVisible = value; 
                PropertyChnaged();
            }
        }
    }
}
