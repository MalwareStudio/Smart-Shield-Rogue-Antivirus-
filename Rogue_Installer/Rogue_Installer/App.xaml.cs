using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Rogue_Installer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            OsDetection();

            // WPF UI thread exceptions
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Non-UI thread exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
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

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"WPF Dispatcher Exception:\n{e.Exception}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true; // prevent crash (optional)
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"AppDomain Exception:\n{e.ExceptionObject}", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
