using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Rogue_Installer.MVVM.ViewModel
{
    public class vmAboutPage : vmBase
    {
		public static vmAboutPage sharedVmAboutPage {  get; } = new vmAboutPage();

		private string specText = "Smart Shield Antivirus was not meant to be legitimate software but rather malicious software that forces users to use this software without an option to quit. Smart Shield is not like any other rogue. Many of these kinds are easy to terminate and get rid of; however, this does not apply to Smart Shield thanks to the complex technique that launches itself on startup." + Environment.NewLine + 
			"Any attempt to terminate this rogue will cause a BSOD (except restarting the computer)." + Environment.NewLine + 
			"If you manage to discover a generated copy of this program and delete it, it's not over. Smart Shield creates its copies multiple times, in random directories with random names. Most of these copies are just \"decoy files.\" If you delete them, they will be generated again." + Environment.NewLine + 
			"There is a legitimate option to quit this rogue, but you would have to purchase the license. The license key is hidden somewhere, so I wish you good luck in finding it :D" + Environment.NewLine + 
			"Be also aware of illegal keywords such as \"avast\", \"virus\", \"kaspersky\" and so on ... If you type these words anywhere on the computer, Smart Shield will notice it and become very upset, so do not try it!" + Environment.NewLine + 
			"Trial Mode is the default mode, which is limited to 7 days. Passing the last day will cause some troubles. If you want to make it happen quickly, just change the datetime forward and you will see ;)";

		public string SpecText
        {
			get { return specText; }
			set 
			{ 
				specText = value;
				PropertyChnaged();
			}
		}

		private BitmapImage animBackground;

		public BitmapImage AnimBackground
        {
			get { return animBackground; }
			set 
			{ 
				animBackground = value; 
				PropertyChnaged();
			}
		}

	}
}
