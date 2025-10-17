using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.MVVM;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmLicensePage : VmBase
    {
		private string windowTitle = "Activation status: not active";

		public string WindowTitle
		{
			get { return windowTitle; }
			set 
			{ 
				windowTitle = value;
				PropertyChnaged();
			}
		}

        private string contentTextBox = "0000-1111-2222-3333";

        public string ContentTextBox
        {
            get { return contentTextBox; }
            set
            {
                contentTextBox = value;
                PropertyChnaged();
            }
        }

        private int caretIndex;

        public int CaretIndex
        {
            get { return caretIndex; }
            set
            {
                caretIndex = value;
                PropertyChnaged();
            }
        }

        private string contentKeyInsert;

        public string ContentKeyInsert
        {
            get { return contentKeyInsert; }
            set
            {
                contentKeyInsert = value;
                PropertyChnaged();
            }
        }

        private LinearGradientBrush colorKeyInsert = mGradient.RedFade();

        public LinearGradientBrush ColorKeyInsert
        {
            get { return colorKeyInsert; }
            set 
            {
                colorKeyInsert = value;
                PropertyChnaged();
            }
        }

        private SolidColorBrush colorTextBox = new SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 100, 100));

        public SolidColorBrush ColorTextBox
        {
            get { return colorTextBox; }
            set
            {
                colorTextBox = value;
                PropertyChnaged();
            }
        }

        private Visibility isVisibleKeyInsert;

        public Visibility IsVisibleKeyInsert
        {
            get { return isVisibleKeyInsert; }
            set 
            { 
                isVisibleKeyInsert = value;
                PropertyChnaged();
            }
        }

    }
}
