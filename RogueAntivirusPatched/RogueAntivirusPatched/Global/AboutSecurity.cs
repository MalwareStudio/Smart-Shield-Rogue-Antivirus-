using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RogueAntivirusPatched.View.Pages;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using RogueAntivirusPatched.ViewModel;
using RogueAntivirusPatched.Model;
using static RogueAntivirusPatched.Global.Convertor;

namespace RogueAntivirusPatched.Global
{
    public class AboutSecurity
    {
        private KeySender keySender;

        public AboutSecurity()
        {
            keySender = new KeySender();
        }

        public class OutputThreatData
        {
            public const string Vector = Variables.RogueBaseDir + "\\" + "avVector.txt";
            public const string Type = Variables.RogueBaseDir + "\\" + "avType.txt";
            public const string Location = Variables.RogueBaseDir + "\\" + "avLocation.txt";
            public const string Level = Variables.RogueBaseDir + "\\" + "avLevel.txt";
        }

        public List<string> ClassToArray()
        {
            var outputData = new OutputThreatData();
            var dataContainer = new List<string>();

            foreach (var item in outputData.GetType().GetFields())
            {
                if (item.FieldType == typeof(string))
                {
                    string data = item.GetValue(outputData) as string;
                    dataContainer.Add(data);
                }
            }

            return dataContainer;
        }

        public void DetermineStatus()
        {
            var list = ClassToArray();
            int count = 0;

            foreach (var file in list)
            {
                if (File.Exists(file))
                    count++;
            }

            if (count > 0)
            {
                ThreatsFound();
                return;
            }

            if (!keySender.HasLicense)
            {
                NotRegistered();
                return;
            }

            NoThreatsFound();
        }

        private void NotRegistered()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.VmMainWindow.LicenseStatusImage = vmMainWindow.defaultLicenseStatusImage;
                mainWindow.VmMainWindow.LicenseStatusTitle = vmMainWindow.defaultLicenseStatusTitle;
                mainWindow.VmMainWindow.LicenseStatusText = vmMainWindow.defaultLicenseStatusText;
                mainWindow.VmMainWindow.ColorBorderStatus = mGradient.OrangeGradient();
            });
        }

        private void NoThreatsFound()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.VmMainWindow.LicenseStatusImage = ToBitmapImagePNG(Properties.Resources.satisfied_shield);
                mainWindow.VmMainWindow.LicenseStatusTitle = "Your PC is Secured!";
                mainWindow.VmMainWindow.LicenseStatusText = "Congratulations! No threats have been found.";
                mainWindow.VmMainWindow.ColorBorderStatus = mGradient.GreenGradient();
            });
        }

        private void ThreatsFound()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.VmMainWindow.LicenseStatusImage = ToBitmapImagePNG(Properties.Resources.furious_shield);
                mainWindow.VmMainWindow.LicenseStatusTitle = "Your PC is Infected!";
                mainWindow.VmMainWindow.LicenseStatusText = "Threats have been found! Remove them immediately!";
                mainWindow.VmMainWindow.ColorBorderStatus = mGradient.RedGradient();
            });
        }
    }
}
