using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RogueAntivirusPatched.MVVM;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmPopup : VmBase
    {
		private string title = "Title";

		public string Title
		{
			get { return title; }
			set 
			{
                title = value;
				PropertyChnaged();
			}
		}

        private string subTitle = "Oh shit";

        public string SubTitle
        {
            get { return subTitle; }
            set
            {
                subTitle = value;
                PropertyChnaged();
            }
        }

        private string text = "This is not good!";

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                PropertyChnaged();
            }
        }

        private BitmapImage picture;

        public BitmapImage Picture
        {
            get { return picture; }
            set 
            {
                picture = value;
                PropertyChnaged();
            }
        }

        private string contentButton = "Remove Threats";

        public string ContentButton
        {
            get { return contentButton; }
            set 
            { 
                contentButton = value; 
                PropertyChnaged();
            }
        }

    }
}
