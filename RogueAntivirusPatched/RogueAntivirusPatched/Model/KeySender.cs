using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RogueAntivirusPatched.Model
{
    internal class KeySender
    {
        public static string regLicenseKey = "License Key";
        private static string subKeyName = AppDomain.CurrentDomain.FriendlyName;
        public static string fixedsubKeyName = subKeyName.Replace(".exe", "");
        public static string mainPath = @"SOFTWARE\" + fixedsubKeyName;

        private bool hasLicense;

        public bool HasLicense
        {
            get 
            {
                hasLicense = IsRegistryKeyValid();
                return hasLicense; 
            }
        }


        public void RegistrySetKey(string licenseKey)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey regKey = baseKey.CreateSubKey(mainPath))
                {
                    regKey.SetValue(regLicenseKey, licenseKey);
                }
            }
        }
        private bool IsRegistryKeyValid()
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey regKey = baseKey.OpenSubKey(mainPath))
                {
                    if (regKey == null)
                        return false;

                    if (regKey.ValueCount == 0)
                        return false;

                    object rawData = regKey.GetValue(regLicenseKey);

                    if (rawData == null)
                        return false;

                    string data = rawData.ToString();
                    foreach (string key in mLicenseKeys.licenseKeys)
                    {
                        if (key.Equals(data))
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
