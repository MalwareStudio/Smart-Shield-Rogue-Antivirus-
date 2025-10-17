using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using RogueAntivirusPatched.MVVM;
using System.Drawing;
using static RogueAntivirusPatched.Global.Convertor;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmAdvertisement : VmBase
    {
		private double width = 800.0;

        public double Width
		{
			get { return width; }
			set 
			{ 
				width = value;
				PropertyChnaged();
			}
		}

        private double height = 800.0;

        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                PropertyChnaged();
            }
        }

        private string description = "This is how the description will look.";

        public string Description
        {
            get { return description; }
            set 
            { 
                description = value; 
                PropertyChnaged();
            }
        }

        private string titlePage = "Special Offer Example";

        public string TitlePage
        {
            get { return titlePage; }
            set
            {
                titlePage = value;
                PropertyChnaged();
            }
        }

        private string contentFirstBtn = "Yes";

        public string ContentFirstBtn
        {
            get { return contentFirstBtn; }
            set
            {
                contentFirstBtn = value;
                PropertyChnaged();
            }
        }

        private string contentSecondBtn = "No";

        public string ContentSecondBtn
        {
            get { return contentSecondBtn; }
            set
            {
                contentSecondBtn = value;
                PropertyChnaged();
            }
        }

        private string textBody = "Say something about this great deal ...";

        public string TextBody
        {
            get { return textBody; }
            set
            {
                textBody = value;
                PropertyChnaged();
            }
        }

        private BitmapImage imageSource = ToBitmapImagePNG(Properties.Resources.blue_emoji_stupid);

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set 
            { 
                imageSource = value; 
                PropertyChnaged();
            }
        }

        private int imageWidth = 150;

        public int ImageWidth
        {
            get { return imageWidth; }
            set 
            { 
                imageWidth = value; 
                PropertyChnaged();
            }
        }

        private int imageHeight = 150;

        public int ImageHeight
        {
            get { return imageHeight; }
            set
            {
                imageHeight = value;
                PropertyChnaged();
            }
        }

        private double imageOpacity;

        public double ImageOpacity
        {
            get { return imageOpacity; }
            set 
            { 
                imageOpacity = value; 
                PropertyChnaged();
            }
        }


        private SolidColorBrush backgroundColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

        public SolidColorBrush BackgroundColor
        {
            get { return backgroundColor; }
            set 
            { 
                backgroundColor = value; 
                PropertyChnaged();
            }
        }


        public ObservableCollection<BitmapImage> MultipleImages { get; set; }

        public vmAdvertisement()
        {
            MultipleImages = new ObservableCollection<BitmapImage>();
        }
    }
}
