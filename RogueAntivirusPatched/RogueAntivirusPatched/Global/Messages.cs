using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RogueAntivirusPatched.Global
{
    public static class Messages
    {
        public static void ProcessIsRunning()
        {
            MessageBox.Show("Wait for the process before it's finished!", "Process is running", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
