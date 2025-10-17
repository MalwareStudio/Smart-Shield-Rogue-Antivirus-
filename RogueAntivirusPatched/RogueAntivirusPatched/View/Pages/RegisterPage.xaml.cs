using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using RogueAntivirusPatched.View.Windows;
using RogueAntivirusPatched.Global;

namespace RogueAntivirusPatched.View.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_Email(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void CopyAddress_Click(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Contact.emailAddress);
        }

        private void Upgrade_Click(object sender, MouseButtonEventArgs e)
        {
            var licenseWindow = new LicenseWindow();
            licenseWindow.ShowDialog();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Contact.GetToEmail();
        }
    }
}
