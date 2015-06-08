using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace TabularDataPackage
{
    /// <summary>
    /// Interaction logic for UserInterface.xaml
    /// </summary>
    public partial class UserInterface : Window
    {
        private readonly FolderBrowserDialog openFileDialog;

        public UserInterface()
        {
            InitializeComponent();
            openFileDialog = new FolderBrowserDialog();
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            string defaultDir = GetGitHubDirectory;
            if (string.IsNullOrEmpty(defaultDir))
                dialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            else
                dialog.SelectedPath = defaultDir;
            DialogResult result = dialog.ShowDialog();
            pathBox.Text = dialog.SelectedPath;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(pathBox.Text))
            {
                button.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = false;
            }
        }

        public string GetGitHubDirectory
        {
            get
            {
                string githubDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GitHub");
                if (Directory.Exists(githubDir))
                    return githubDir;
                else
                    return null;
            }
        }
    }
}
