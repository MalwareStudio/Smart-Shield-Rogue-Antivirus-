using RogueAntivirusPatched.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmCommonButton : VmBase
    {
		private double setFontSize = 14.0;

		public double SetFontSize
        {
			get { return setFontSize; }
			set 
			{ 
				setFontSize = value;
				PropertyChnaged();
			}
		}

        private string setContent = "Hello World";

        public string SetContent
        {
            get { return setContent; }
            set
            {
                setContent = value;
                PropertyChnaged();
            }
        }

        private double setWidth = 100;
        public double SetWidth
        {
            get { return setWidth; }
            set
            {
                setWidth = value;
                PropertyChnaged();
            }
        }

        private double setHeight = 50;
        public double SetHeight
        {
            get { return setHeight; }
            set
            {
                setHeight = value;
                PropertyChnaged();
            }
        }

        private bool setIsEnabled = true;
        public bool SetIsEnabled
        {
            get { return setIsEnabled; }
            set
            {
                setIsEnabled = value;
                PropertyChnaged();
            }
        }
    }
}
