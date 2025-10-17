using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmJunkCleanerPage : VmBase
    {
		private bool isTemp = true;

		public bool IsTemp
		{
			get { return isTemp; }
			set 
			{ 
				isTemp = value;
				PropertyChnaged();
			}
		}

        private string contentTemp = "Temporary Files";

        public string ContentTemp
        {
            get { return contentTemp; }
            set
            {
                contentTemp = value;
                PropertyChnaged();
            }
        }


        private bool isMemory = true;

        public bool IsMemory
        {
            get { return isMemory; }
            set
            {
                isMemory = value;
                PropertyChnaged();
            }
        }

        private string contentMemory = "Memory Dumps";

        public string ContentMemory
        {
            get { return contentMemory; }
            set
            {
                contentMemory = value;
                PropertyChnaged();
            }
        }

        private bool isLogs = true;

        public bool IsLogs
        {
            get { return isLogs; }
            set
            {
                isLogs = value;
                PropertyChnaged();
            }
        }

        private string contentLogs = "Windows Log Files";

        public string ContentLogs
        {
            get { return contentLogs; }
            set
            {
                contentLogs = value;
                PropertyChnaged();
            }
        }

        private bool isChkdks = true;

        public bool IsChkdsk
        {
            get { return isChkdks; }
            set
            {
                isChkdks = value;
                PropertyChnaged();
            }
        }

        private string contentChkdks = "Chkdsk File Fragments";

        public string ContentChkdks
        {
            get { return contentChkdks; }
            set
            {
                contentChkdks = value;
                PropertyChnaged();
            }
        }

        private bool isCache = true;

        public bool IsCache
        {
            get { return isCache; }
            set
            {
                isCache = value;
                PropertyChnaged();
            }
        }

        private string contentCache = "Cache";

        public string ContentCache
        {
            get { return contentCache; }
            set
            {
                contentCache = value;
                PropertyChnaged();
            }
        }

        private bool isError = true;

        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                PropertyChnaged();
            }
        }

        private string contentError = "Error Reports";

        public string ContentError
        {
            get { return contentError; }
            set
            {
                contentError = value;
                PropertyChnaged();
            }
        }

        private bool isStartMenu = true;

        public bool IsStartMenu
        {
            get { return isStartMenu; }
            set
            {
                isStartMenu = value;
                PropertyChnaged();
            }
        }

        private string contentStartMenu = "Start Menu Shortcuts";

        public string ContentStartMenu
        {
            get { return contentStartMenu; }
            set
            {
                contentStartMenu = value;
                PropertyChnaged();
            }
        }

        private bool isStartUp = true;

        public bool IsStartUp
        {
            get { return isStartUp; }
            set
            {
                isStartUp = value;
                PropertyChnaged();
            }
        }

        private string contentStartUp = "Startup Programs";

        public string ContentStartUp
        {
            get { return contentStartUp; }
            set
            {
                contentStartUp = value;
                PropertyChnaged();
            }
        }

        private string processBarPercentage = "0%";

        public string ProcessBarPercentage
        {
            get { return processBarPercentage; }
            set 
            { 
                processBarPercentage = value;
                PropertyChnaged();
            }
        }

        private double processBarValue = 0;

        public double ProcessBarValue
        {
            get { return processBarValue; }
            set 
            { 
                processBarValue = value; 
                PropertyChnaged();
            }
        }


        private ObservableCollection<mTempInfo> tempInfo = new ObservableCollection<mTempInfo>();

        public ObservableCollection<mTempInfo> TempInfo
        {
            get { return tempInfo; }
            set
            {
                tempInfo = value;
                PropertyChnaged();
            }
        }

        private string aboutProgress = "Search for unnecessary files via the \"Analyze\" Button";

        public string AboutProgress
        {
            get { return aboutProgress; }
            set 
            { 
                aboutProgress = value;
                PropertyChnaged();
            }
        }

        private string progressDetails = "Details will be revealed here";

        public string ProgressDetails
        {
            get { return progressDetails; }
            set 
            { 
                progressDetails = value;
                PropertyChnaged();
            }
        }

        private bool canAnalyze = true;

        public bool CanAnalyze
        {
            get { return canAnalyze; }
            set 
            { 
                canAnalyze = value;
                PropertyChnaged();
            }
        }

        private bool canRunCleaner = false;

        public bool CanRunCleaner
        {
            get { return canRunCleaner; }
            set
            {
                canRunCleaner = value;
                PropertyChnaged();
            }
        }

        private bool isStandard = true;

        public bool IsStandard
        {
            get { return isStandard; }
            set 
            { 
                isStandard = value;
                PropertyChnaged();
            }
        }

        private bool isAdvanced;

        public bool IsAdvanced
        {
            get { return isAdvanced; }
            set
            {
                isAdvanced = value;
                PropertyChnaged();
            }
        }

        private bool toggleButtons = true;

        public bool ToggleButtons
        {
            get { return toggleButtons; }
            set 
            { 
                toggleButtons = value; 
                PropertyChnaged();
            }
        }

    }
}
