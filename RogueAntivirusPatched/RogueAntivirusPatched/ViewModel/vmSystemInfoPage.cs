using RogueAntivirusPatched.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RogueAntivirusPatched.ViewModel
{
    internal class vmSystemInfoPage : VmBase
    {
        private string osName;
        public string OsName
        {
            get { return osName; }
            set
            {
                osName = value;
                PropertyChnaged();
            }
        }

        private string osVersion;
        public string OsVersion
        {
            get { return osVersion; }
            set
            {
                osVersion = value;
                PropertyChnaged();
            }
        }

        private string osBuild;
        public string OsBuild
        {
            get { return osBuild; }
            set
            {
                osBuild = value;
                PropertyChnaged();
            }
        }

        private string osArch;
        public string OsArch
        {
            get { return osArch; }
            set
            {
                osArch = value;
                PropertyChnaged();
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChnaged();
            }
        }

        private string computerName;
        public string ComputerName
        {
            get { return computerName; }
            set
            {
                computerName = value;
                PropertyChnaged();
            }
        }

        private string cpuName;
        public string CpuName
        {
            get { return cpuName; }
            set
            {
                cpuName = value;
                PropertyChnaged();
            }
        }

        private string cpuSpeed;
        public string CpuSpeed
        {
            get { return cpuSpeed; }
            set
            {
                cpuSpeed = value;
                PropertyChnaged();
            }
        }

        private string cpuCores;
        public string CpuCores
        {
            get { return cpuCores; }
            set
            {
                cpuCores = value;
                PropertyChnaged();
            }
        }

        private string pcModel;
        public string PcModel
        {
            get { return pcModel; }
            set
            {
                pcModel = value;
                PropertyChnaged();
            }
        }

        private string brand;
        public string Brand
        {
            get { return brand; }
            set
            {
                brand = value;
                PropertyChnaged();
            }
        }

        private string biosVersion;
        public string BiosVersion
        {
            get { return biosVersion; }
            set
            {
                biosVersion = value;
                PropertyChnaged();
            }
        }

        private string selDisk;
        public string SelDisk
        {
            get { return selDisk; }
            set
            {
                selDisk = value;
                PropertyChnaged();
            }
        }

        private string diskType;
        public string DiskType
        {
            get { return diskType; }
            set
            {
                diskType = value;
                PropertyChnaged();
            }
        }

        private string diskFormat;
        public string DiskFormat
        {
            get { return diskFormat; }
            set
            {
                diskFormat = value;
                PropertyChnaged();
            }
        }

        private string diskSize;
        public string DiskSize
        {
            get { return diskSize; }
            set
            {
                diskSize = value;
                PropertyChnaged();
            }
        }

        private string diskFree;
        public string DiskFree
        {
            get { return diskFree; }
            set
            {
                diskFree = value;
                PropertyChnaged();
            }
        }
    }
}
