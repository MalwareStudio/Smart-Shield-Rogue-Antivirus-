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
    internal class vmAnnoyingPopUp : VmBase
    {
		private string title = "License Alert";

		public string Title
		{
			get { return title; }
			set 
			{ 
				title = value;
				PropertyChnaged();
			}
		}

        private string subTitle = "Trial Mode Will Expire!";

        public string SubTitle
        {
            get { return subTitle; }
            set
            {
                subTitle = value;
                PropertyChnaged();
            }
        }

        private string text = "Some text";

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                PropertyChnaged();
            }
        }

        private BitmapImage popImage = ToBitmapImagePNG(Properties.Resources.warning);

        public BitmapImage PopImage
        {
            get { return popImage; }
            set 
            { 
                popImage = value;
                PropertyChnaged();
            }
        }

        private string contentButtonFirst = "Yes";

        public string ContentButtonFirst
        {
            get { return contentButtonFirst; }
            set 
            { 
                contentButtonFirst = value; 
                PropertyChnaged();
            }
        }

        private string contentButtonSecond = "No";

        public string ContentButtonSecond
        {
            get { return contentButtonSecond; }
            set
            {
                contentButtonSecond = value;
                PropertyChnaged();
            }
        }

        private Thickness imageMargin = new Thickness(0, 0, 0, 80);

        public Thickness ImageMargin
        {
            get { return imageMargin; }
            set 
            { 
                imageMargin = value; 
                PropertyChnaged();
            }
        }

        private int gridWidth = 500;

        public int GridWidth
        {
            get { return gridWidth; }
            set 
            { 
                gridWidth = value; 
                PropertyChnaged();
            }
        }

        private int gridHeight = 400;

        public int GridHeight
        {
            get { return gridHeight; }
            set
            {
                gridHeight = value;
                PropertyChnaged();
            }
        }
    }
}
