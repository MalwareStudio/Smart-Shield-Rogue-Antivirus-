using RogueAntivirusPatched.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueAntivirusPatched.View;
using RogueAntivirusPatched.ViewModel;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using RogueAntivirusPatched.View.Pages;
using System.Windows.Media.Imaging;
using RogueAntivirusPatched.Global;
using static RogueAntivirusPatched.Global.Convertor;

namespace RogueAntivirusPatched.ViewModel
{
    public class vmMainWindow : VmBase
    {
        private vmMenuItem _vmMenuItem;

		public vmMenuItem VmMenuItem
        {
			get { return _vmMenuItem; }
			set 
			{
                _vmMenuItem = value;
				PropertyChnaged();
			}
		}

		private string windowTitle = "amogus";

		public string WindowTitle
		{
			get { return windowTitle; }
			set 
			{ 
				windowTitle = value;
				PropertyChnaged();
			}
		}


		private Visibility isVisibleRegisterBtn = Visibility.Hidden;

		public Visibility IsVisibleRegisterBtn
        {
			get { return isVisibleRegisterBtn; }
			set 
			{ 
				isVisibleRegisterBtn = value;
				PropertyChnaged();
			}
		}

		private LinearGradientBrush colorBorderStatus;


        public LinearGradientBrush ColorBorderStatus
        {
			get { return colorBorderStatus; }
			set 
			{ 
				colorBorderStatus = value;
				PropertyChnaged();
			}
		}

		private Page contentPage;

		public Page ContentPage
        {
			get { return contentPage; }
			set 
			{ 
				contentPage = value;
				PropertyChnaged();
			}
		}

		private bool isSafe = true;
		public bool IsSafe
        {
			get 
			{
                return isSafe; 
			}
			set
			{
				isSafe = value;
				PropertyChnaged();
			}
		}

		public static string defaultLicenseStatusTitle = "Your Pc is in Danger!";

        private string licenseStatusTitle = defaultLicenseStatusTitle;

		public string LicenseStatusTitle
        {
			get { return licenseStatusTitle; }
			set 
			{ 
				licenseStatusTitle = value; 
				PropertyChnaged();
			}
		}

		public static string defaultLicenseStatusText = "Purchase the License and use all security features";

        private string licenseStatusText = defaultLicenseStatusText;

        public string LicenseStatusText
        {
            get { return licenseStatusText; }
            set
            {
                licenseStatusText = value;
                PropertyChnaged();
            }
        }

		public static BitmapImage defaultLicenseStatusImage = ToBitmapImagePNG(Properties.Resources.awkward_shield);


        private BitmapImage licenseStatusImage = defaultLicenseStatusImage;

		public BitmapImage LicenseStatusImage
        {
			get { return licenseStatusImage; }
			set 
			{ 
				licenseStatusImage = value;
				PropertyChnaged();
			}
		}

		private BitmapImage appLogo = ToBitmapImagePNG(Properties.Resources.happy_shield);

		public BitmapImage AppLogo
        {
			get { return appLogo; }
			set 
			{ 
				appLogo = value; 
				PropertyChnaged();
			}
		}

		private string appName;

		public string AppName
        {
			get { return appName; }
			set 
			{ 
				appName = value; 
				PropertyChnaged();
			}
		}

	}
}
