using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using RogueAntivirusPatched.MVVM;
using RogueAntivirusPatched.ViewModel;

namespace RogueAntivirusPatched.View.Pages
{
    /// <summary>
    /// Interaction logic for SystemInfoPage.xaml
    /// </summary>
    public partial class SystemInfoPage : Page
    {
        private vmSystemInfoPage _vmSystemInfoPage;
        public SystemInfoPage()
        {
            InitializeComponent();
            _vmSystemInfoPage = new vmSystemInfoPage();
            DataContext = _vmSystemInfoPage;

            _vmSystemInfoPage.OsName = SysInfo.SimpleVersion();
            _vmSystemInfoPage.OsVersion = SysInfo.Version;
            _vmSystemInfoPage.OsBuild = SysInfo.Build;
            _vmSystemInfoPage.OsArch = SysInfo.Is64Bit();

            _vmSystemInfoPage.Username = SysInfo.Username;
            _vmSystemInfoPage.ComputerName = SysInfo.ComputerName;

            _vmSystemInfoPage.CpuName = SysInfo.CpuName();
            _vmSystemInfoPage.CpuSpeed = SysInfo.CpuGhz();
            _vmSystemInfoPage.CpuCores = SysInfo.Threads;
            _vmSystemInfoPage.PcModel = SysInfo.SystemFamily();
            _vmSystemInfoPage.Brand = SysInfo.Brand();

            _vmSystemInfoPage.BiosVersion = SysInfo.BiosVersion();
            string targetDisk = @"C:\";
            _vmSystemInfoPage.SelDisk = Drive.Type(targetDisk);
            _vmSystemInfoPage.DiskType = Drive.Type(targetDisk);
            _vmSystemInfoPage.DiskFormat = Drive.Format(targetDisk);
            _vmSystemInfoPage.DiskSize = Drive.TotalSize(targetDisk);
            _vmSystemInfoPage.DiskFree = Drive.FreeSpace(targetDisk);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private static class SysInfo
        {
            //Software
            public static string SimpleVersion()
            {
                var compInfo = new ComputerInfo();
                string os = compInfo.OSFullName.Replace("Microsoft", "");
                string result = os.TrimStart(' ');

                return result;
            }
            public static string TotalRam()
            {
                var compInfo = new ComputerInfo();
                ulong ram = compInfo.TotalPhysicalMemory;
                double toGbram = (double)ram / 1000000000;
                double fixedRam = Math.Round(toGbram, 0);

                return fixedRam.ToString();
            }

            public static string Version = Environment.OSVersion.VersionString;
            public static string Build = Environment.Version.ToString();
            public static string Is64Bit()
            {
                if (Environment.Is64BitOperatingSystem)
                    return "x64";

                return "x86";
            }
            public static string ComputerName = Environment.MachineName;
            public static string Username = Environment.UserName;

            //Hardware
            public static string Threads = Environment.ProcessorCount.ToString();
            static string CpuPath = "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0";
            public static string CpuName()
            {
                using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(CpuPath))
                {
                    object objCpu = reg.GetValue("ProcessorNameString");

                    return objCpu.ToString();
                }
            }
            public static string CpuGhz()
            {
                using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(CpuPath))
                {
                    object objCpu = reg.GetValue("~MHz");
                    int toInt32 = Convert.ToInt32(objCpu.ToString());
                    double toGhz = (double)toInt32 / 1000;
                    double fixGhz = Math.Round(toGhz, 2);

                    return fixGhz.ToString();
                }
            }

            static string BiosPath = "HARDWARE\\DESCRIPTION\\System\\BIOS";
            public static string SystemFamily()
            {
                using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(BiosPath))
                {
                    object objBios = reg.GetValue("SystemFamily");

                    return objBios.ToString();
                }
            }
            public static string BiosVersion()
            {
                using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(BiosPath))
                {
                    object objBios = reg.GetValue("BIOSVersion");

                    return objBios.ToString();
                }
            }
            public static string Brand()
            {
                using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(BiosPath))
                {
                    object objBios = reg.GetValue("BaseBoardManufacturer");

                    return objBios.ToString();
                }
            }
        }
        public class Drive
        {
            public static string Type(string drive)
            {
                var driveInfo = new DriveInfo(drive);
                return driveInfo.DriveType.ToString();
            }
            public static string Format(string drive)
            {
                var driveInfo = new DriveInfo(drive);
                return driveInfo.DriveFormat.ToString();
            }
            public static string Label(string drive)
            {
                var driveInfo = new DriveInfo(drive);
                return driveInfo.VolumeLabel.ToString();
            }
            public static string TotalSize(string drive)
            {
                var driveInfo = new DriveInfo(drive);
                long size = driveInfo.TotalSize;
                double toGb = size / Math.Pow(1024, 3);
                double fixGb = Math.Round(toGb, 0);

                return fixGb.ToString();
            }
            public static string FreeSpace(string drive)
            {
                var driveInfo = new DriveInfo(drive);
                long size = driveInfo.TotalFreeSpace;
                double toGb = size / Math.Pow(1024, 3);
                double fixGb = Math.Round(toGb, 1);

                return fixGb.ToString();
            }
        }
    }
}
