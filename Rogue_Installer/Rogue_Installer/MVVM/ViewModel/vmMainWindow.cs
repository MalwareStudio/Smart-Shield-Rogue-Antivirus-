using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Rogue_Installer.MVVM;
using forms = System.Windows.Forms;
using static Rogue_Installer.MVVM.Model.Global;

namespace Rogue_Installer.MVVM.ViewModel
{
    public class vmMainWindow : vmBase
    {
		private string appName = AppDomain.CurrentDomain.FriendlyName + " - " + pageTitle;

		public string AppName
		{
			get { return appName; }
			set 
			{ 
				appName = value;
				PropertyChnaged();
			}
		}

		private static string pageTitle = "Something";

		public string PageTitle
        {
			get { return pageTitle; }
			set 
			{ 
				pageTitle = value;
				if (PageTitle != "Warning")
				{
                    AppName = AppDomain.CurrentDomain.FriendlyName + " - " + PageTitle;
                }
				else
				{
                    AppName = AppDomain.CurrentDomain.FriendlyName + " - " + MainMenuContent;
                }
                PropertyChnaged();
			}
		}

		private string importantText = "To prevent any misunderstanding, please read the text below " +
            "until the end!" + Environment.NewLine +
			"We are not responsible for any damages!";

		public string ImportantText
        {
			get { return importantText; }
			set 
			{ 
				importantText = value;
                PropertyChnaged();
            }
		}


		private string contentText = "You are about to run a dangerous program that was made for " +
            "pure demonstration intended for virtual machines (vms). " + Environment.NewLine +
            "DO NOT RUN THIS MALICIOUS PROGRAM ON YOUR DEVICE! THIS IS NOT A JOKE! ONCE YOU RUN IT, " +
            "THERE IS NO WAY OF RETURNING YOUR SYSTEM SETTINGS BACK! BY RUNNING THIS PROGRAM YOU WILL " +
            "ALSO PUT YOUR PERSONAL FILES AT RISK!" + Environment.NewLine + Environment.NewLine +
            @"""Don't worry, this software will not spread itself onto other devices. Keep it inside the safe environment!""" + Environment.NewLine + Environment.NewLine +
            "This quote is true. We made this software just for fun; this software is not meant to cause " +
            "problems for other people outside of our malware community. If you are curious, then run it at your own " +
            "risk and have fun. As long as you don't damage your device and don't spread it to other machines you " +
            "are good to launch this program. Keep in mind that spreading malware or any kind of malicious software is against the law in many countries, and it can " +
            "bring you some problems with the law, so don't even think about doing it! " + Environment.NewLine + Environment.NewLine +
            "If you are fully aware of all information written above this sentence, then please choose these two options below " +
            "according to your situation on this device." + Environment.NewLine;

		public string ContentText
		{
			get { return contentText; }
			set 
			{ 
				contentText = value; 
				PropertyChnaged();
			}
		}

		private Page currentPage;

		public Page CurrentPage
        {
			get { return currentPage; }
			set 
			{ 
				currentPage = value;
				PropertyChnaged();
			}
		}

		private string aboutContent = "About";

		public string AboutContent
        {
			get { return aboutContent; }
			set 
			{ 
				aboutContent = value; 
				PropertyChnaged();
			}
		}

        private string creditsContent = "Credits";

        public string CreditsContent
        {
            get { return creditsContent; }
            set
            {
                creditsContent = value;
                PropertyChnaged();
            }
        }

        private string followContent = "Follow Us";

        public string FollowContent
        {
            get { return followContent; }
            set
            {
                followContent = value;
                PropertyChnaged();
            }
        }

        private string mainMenuContent = "Main Menu";

        public string MainMenuContent
        {
            get { return mainMenuContent; }
            set
            {
                mainMenuContent = value;
                PropertyChnaged();
            }
        }

        private string lastWarnContent = "Last Warning";

        public string LastWarnContent
        {
            get { return lastWarnContent; }
            set
            {
                lastWarnContent = value;
                PropertyChnaged();
            }
        }

		private string textLastWarning = "Because we want to keep you all in safety, here is the Last Warning. " +
            "You might have clicked on the Installation button by mistake. We are very aware of this, so that's why " +
            @"we brought to you the last option, which can close this window. If you click on the ""Exit"" button, this installer will " +
            "close itself and prevent any harm to this device." + Environment.NewLine + Environment.NewLine +
			"We are asking you once again." + Environment.NewLine +
            @"DO YOU ACTUALLY WANT TO LAUNCH THIS MALICIOUS SOFTWARE? BY CLICKING ON ""Install"", " +
            "YOU ARE BRINGING THIS WHOLE DEVICE INTO DANGER, AND THERE WILL NOT BE ANY OTHER OPTION TO QUIT THIS MALICIOUS SOFTWARE!";

		public string TextLastWarning
        {
			get { return textLastWarning; }
			set 
			{ 
				textLastWarning = value;
				PropertyChnaged();
			}
		}

        private string lastImportantMessage = "GOD BLESS YOU IF YOU DECIDED TO RUN IT ON YOUR OWN HARDWARE!";

        public string LastImportantMessage
        {
            get { return lastImportantMessage; }
            set
            {
                lastImportantMessage = value;
                PropertyChnaged();
            }
        }

        private BitmapImage randomBackground;

		public BitmapImage RandomBackground
        {
			get { return randomBackground; }
			set 
			{ 
				randomBackground = value; 
				PropertyChnaged();
			}
		}

		private BitmapImage volumeIcon;

		public BitmapImage VolumeIcon
		{
			get { return volumeIcon; }
			set 
			{ 
				volumeIcon = value;
				PropertyChnaged();
			}
		}

		private bool isMenuEnabled = true;

		public bool IsMenuEnabled
        {
			get { return isMenuEnabled; }
			set 
			{ 
				isMenuEnabled = value; 
				PropertyChnaged();
			}
		}
	}
}
