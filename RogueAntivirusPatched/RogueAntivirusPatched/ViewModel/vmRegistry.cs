using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueAntivirusPatched.MVVM;
using RogueAntivirusPatched.Model;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmRegistry : VmBase
    {
		private bool isEmptyData = true;

		public bool IsEmptyData
        {
			get { return isEmptyData; }
			set 
			{ 
				isEmptyData = value;
				PropertyChnaged();
			}
		}

        private bool isSharedDll = true;

        public bool IsSharedDll
        {
            get { return isSharedDll; }
            set
            {
                isSharedDll = value;
                PropertyChnaged();
            }
        }

        private bool isAppPaths = true;

        public bool IsAppPaths
        {
            get { return isAppPaths; }
            set
            {
                isAppPaths = value;
                PropertyChnaged();
            }
        }

        private bool isUnused = false;

        public bool IsUnused
        {
            get { return isUnused; }
            set
            {
                isUnused = value;
                PropertyChnaged();
            }
        }

        private bool isInvalidUninstaller = false;

        public bool IsInvalidUninstaller
        {
            get { return isInvalidUninstaller; }
            set
            {
                isInvalidUninstaller = value;
                PropertyChnaged();
            }
        }

        private bool isRunAtStart = false;

        public bool IsRunAtStart
        {
            get { return isRunAtStart; }
            set
            {
                isRunAtStart = value;
                PropertyChnaged();
            }
        }

        private string contentEmptyData = "Empty Data";

        public string ContentEmptyData
        {
            get { return contentEmptyData; }
            set 
            { 
                contentEmptyData = value;
                PropertyChnaged();
            }
        }

        private string contentSharedDll = "Missing Shared DLL";

        public string ContentSharedDll
        {
            get { return contentSharedDll; }
            set
            {
                contentSharedDll = value;
                PropertyChnaged();
            }
        }

        private string contentAppPaths = "Application Paths";

        public string ContentAppPaths
        {
            get { return contentAppPaths; }
            set
            {
                contentAppPaths = value;
                PropertyChnaged();
            }
        }

        private string contentUnused = "Unused Extensions (Pro)";

        public string ContentUnused
        {
            get { return contentUnused; }
            set
            {
                contentUnused = value;
                PropertyChnaged();
            }
        }

        private string contentInvalidUninstaller = "Invalid Uninstaller (Pro)";

        public string ContentInvalidUninstaller
        {
            get { return contentInvalidUninstaller; }
            set
            {
                contentInvalidUninstaller = value;
                PropertyChnaged();
            }
        }

        private string contentRunAtStart = "Startup Programs (Pro)";

        public string ContentRunAtStart
        {
            get { return contentRunAtStart; }
            set
            {
                contentRunAtStart = value;
                PropertyChnaged();
            }
        }

        private double progressBarValue = 0.0;

        public double ProgressBarValue
        {
            get { return progressBarValue; }
            set 
            { 
                progressBarValue = value;
                PropertyChnaged();
            }
        }

        private string progressBarContent = "0%";

        public string ProgressBarContent
        {
            get { return progressBarContent; }
            set 
            { 
                progressBarContent = value;
                PropertyChnaged();
            }
        }

        private string aboutProgress = "Search for Useless Registry via the \"Analyze\" button";

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

        private ObservableCollection<mRegistry> registryInfo = new ObservableCollection<mRegistry>();

        public ObservableCollection<mRegistry> RegistryInfo
        {
            get { return registryInfo; }
            set 
            { 
                registryInfo = value;
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

        private bool canRemove = false;

        public bool CanRemove
        {
            get { return canRemove; }
            set
            {
                canRemove = value;
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
