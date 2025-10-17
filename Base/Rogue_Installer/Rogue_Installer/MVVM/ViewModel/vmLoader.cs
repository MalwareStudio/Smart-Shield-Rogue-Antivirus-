using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue_Installer.MVVM.ViewModel
{
    public class vmLoader : vmBase
    {
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

		private string progressDescription = "Working on something ...";

		public string ProgressDescription
        {
			get { return progressDescription; }
			set 
			{ 
				progressDescription = value; 
				PropertyChnaged();
			}
		}

	}
}
