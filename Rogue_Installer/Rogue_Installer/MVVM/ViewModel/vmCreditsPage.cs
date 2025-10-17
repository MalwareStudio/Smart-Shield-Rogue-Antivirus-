using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Rogue_Installer.MVVM.ViewModel
{
    internal class vmCreditsPage : vmBase
    {
		private string authorText = "Founder and the main developer of this project." + Environment.NewLine +
            "Responsible for the idea, UI, and the rest of the functionality of the Smart Shield.";

		public string AuthorText
        {
			get { return authorText; }
			set 
			{ 
				authorText = value;
				PropertyChnaged();
			}
		}

        private string authorText2 = "Second developer who made keylogger, trayicon, spiral window animation (in payloads) and " +
            @"popup windows such as ""Threat Detected"" and ""Advertisement""." + Environment.NewLine +
            "Big shout-out to him as well!";

        public string AuthorText2
        {
            get { return authorText2; }
            set
            {
                authorText2 = value;
                PropertyChnaged();
            }
        }

        private BitmapImage backgroundCyberSoldier;

        public BitmapImage BackgroundCyberSoldier
        {
            get { return backgroundCyberSoldier; }
            set 
            { 
                backgroundCyberSoldier = value; 
                PropertyChnaged();
            }
        }

        private BitmapImage backgroundExlon;

        public BitmapImage BackgroundExlon
        {
            get { return backgroundExlon; }
            set
            {
                backgroundExlon = value;
                PropertyChnaged();
            }
        }
    }
}
