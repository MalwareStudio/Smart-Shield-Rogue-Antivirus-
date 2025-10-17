using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.MVVM;
using Windows.Devices.PointOfService;
using Windows.Foundation.Metadata;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmAntivirusPage : VmBase
    {
        private bool isQuick = true;

        public bool IsQuick
        {
            get { return isQuick; }
            set
            {
                isQuick = value;
                PropertyChnaged();
            }
        }

        private bool isDeep;

        public bool IsDeep
        {
            get { return isDeep; }
            set
            {
                isDeep = value;
                PropertyChnaged();
            }
        }

        private bool isCustom;

        public bool IsCustom
        {
            get { return isCustom; }
            set
            {
                isCustom = value;
                PropertyChnaged();
            }
        }

        public static readonly string defaultTextProcessingFile = "Processing File: ";
        private string textProcessingFile = defaultTextProcessingFile;

        public string TextProcessingFile
        {
            get { return textProcessingFile; }
            set
            {
                textProcessingFile = value;
                PropertyChnaged();
            }
        }

        public static readonly string defaultTextScannedFiles = "Files Scanned: ";
        private string textScannedFiles = defaultTextScannedFiles;

        public string TextScannedFiles
        {
            get { return textScannedFiles; }
            set
            {
                textScannedFiles = value;
                PropertyChnaged();
            }
        }

        public static readonly string defaultTextThreatsDetected = "Threats Detected: ";
        private string textThreatsDetected = defaultTextThreatsDetected;

        public string TextThreatsDetected
        {
            get { return textThreatsDetected; }
            set
            {
                string currentValue = value;
                if (currentValue.Any(char.IsDigit))
                {
                    string getNumber = new string(currentValue.Where(char.IsDigit).ToArray());
                    if (int.TryParse(getNumber, out int num))
                    {
                        if (num > 0)
                            ForegroundThreatsDetected = mGradient.RedFade();
                        else
                            ForegroundThreatsDetected = mGradient.WhiteFade();
                    }
                }
                else
                {
                    ForegroundThreatsDetected = mGradient.WhiteFade();
                }

                textThreatsDetected = value;
                PropertyChnaged();
            }
        }

        public static readonly string defaultTextThreatsRemoved = "Removed: ";
        private string textThreatsRemoved = defaultTextThreatsRemoved;

        public string TextThreatsRemoved
        {
            get { return textThreatsRemoved; }
            set
            {
                textThreatsRemoved = value;
                PropertyChnaged();
            }
        }

        private bool isEnabledStartBtn;

        public bool IsEnabledStartBtn
        {
            get { return isEnabledStartBtn; }
            set
            {
                isEnabledStartBtn = value;
                PropertyChnaged();
            }
        }

        private bool isEnabledStopBtn;

        public bool IsEnabledStopBtn
        {
            get { return isEnabledStopBtn; }
            set
            {
                isEnabledStopBtn = value;
                PropertyChnaged();
            }
        }

        private bool isEnabledPauseBtn;

        public bool IsEnabledPauseBtn
        {
            get { return isEnabledPauseBtn; }
            set
            {
                isEnabledPauseBtn = value;
                PropertyChnaged();
            }
        }

        private string contentPauseBtn = "Pause";

        public string ContentPauseBtn
        {
            get { return contentPauseBtn; }
            set
            {
                contentPauseBtn = value;
                PropertyChnaged();
            }
        }

        private Brush foregroundThreatsDetected = Brushes.White;

        public Brush ForegroundThreatsDetected
        {
            get { return foregroundThreatsDetected; }
            set
            {
                foregroundThreatsDetected = value;
                PropertyChnaged();
            }
        }

        private ObservableCollection<mThreatInfo> threatInfo = new ObservableCollection<mThreatInfo>();

        public ObservableCollection<mThreatInfo> ThreatInfo
        {
            get { return threatInfo; }
            set
            {
                threatInfo = value;
                PropertyChnaged();
            }
        }

        private bool isRemoveThreats = false;

        public bool IsRemoveThreats
        {
            get { return isRemoveThreats; }
            set 
            { 
                isRemoveThreats = value; 
                PropertyChnaged();
            }
        }

        private double barValue = 0;

        public double BarValue
        {
            get { return barValue; }
            set 
            { 
                barValue = value;
                PropertyChnaged();
            }
        }

        public static readonly string defaultProcessStatus = @"Select the ""Scan Mode"" and search for threats via the ""Start"" button";

        private string processStatus = defaultProcessStatus;

        public string ProcessStatus
        {
            get { return processStatus; }
            
            set 
            { 
                processStatus = value;
                PropertyChnaged();
            }
        }

        private bool modesRadialEnabled = true;

        public bool ModesRadialEnabled
        {
            get { return modesRadialEnabled; }
            set 
            { 
                modesRadialEnabled = value; 
                PropertyChnaged();
            }
        }

    }
}
