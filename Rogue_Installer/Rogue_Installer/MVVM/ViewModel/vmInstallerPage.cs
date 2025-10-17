using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Rogue_Installer.MVVM.ViewModel
{
    public class vmInstallerPage : vmBase
    {
        private string aboutProgress;

		public string AboutProgress
        {
			get { return aboutProgress; }
			set 
			{ 
				aboutProgress = value;
				PropertyChnaged();
			}
		}

		private double barValue;

		public double BarValue
        {
			get { return barValue; }
			set 
			{ 
				barValue = value; 
				PropertyChnaged();
			}
		}

        private string barContent = "0%";

        public string BarContent
        {
            get { return barContent; }
            set
            {
                barContent = value;
                PropertyChnaged();
            }
        }

        private string someTips = "Did you know that you can buy a soap you nasty ass?";

        public string SomeTips
        {
            get { return someTips; }
            set
            {
                someTips = value;
                PropertyChnaged();
            }
        }

        private string instructions = "Do not turn off your computer";

        public string Instructions
        {
            get { return instructions; }
            set 
            { 
                instructions = value; 
                PropertyChnaged();
            }
        }

    }
}
