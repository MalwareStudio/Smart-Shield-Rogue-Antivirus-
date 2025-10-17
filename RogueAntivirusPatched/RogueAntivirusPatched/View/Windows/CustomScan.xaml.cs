using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using forms = System.Windows.Forms;
using RogueAntivirusPatched.Global;
using RogueAntivirusPatched.View.Pages;
using Windows.UI.Xaml.Controls;
using AdvancedIO;

namespace RogueAntivirusPatched.View.Windows
{
    /// <summary>
    /// Interaction logic for CustomScan.xaml
    /// </summary>
    public partial class CustomScan : Window
    {
        private advancedIO _advancedIO;
        public CustomScan()
        {
            InitializeComponent();
            _advancedIO = new advancedIO();
        }

        private void BtnFolder_Click(object sender, MouseButtonEventArgs e)
        {
            using (var folderDialog = new forms.FolderBrowserDialog())
            {
                folderDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderDialog.Description = "Choose a specific folder";
                folderDialog.ShowNewFolderButton = true;

                folderDialog.ShowDialog();

                string selectedFolder = folderDialog.SelectedPath;

                var files = _advancedIO.GetAllFilesSafely(true, selectedFolder, "*");
                AntivirusPage.allFiles = files;
            }
            Close();
        }

        private void BtnFiles_Click(object sender, MouseButtonEventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = "Choose specific files";
                fileDialog.AddExtension = true;
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;
                fileDialog.Filter = "All files (*.*)|*.*";
                fileDialog.Multiselect = true;
                fileDialog.FilterIndex = 0;
                fileDialog.DefaultExt = ".exe";

                fileDialog.ShowDialog();

                string[] selectedFiles = fileDialog.FileNames;

                AntivirusPage.allFiles = selectedFiles.ToList();
            }
            Close();
        }
    }
}
