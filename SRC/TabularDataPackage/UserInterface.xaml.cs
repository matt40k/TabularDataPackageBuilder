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
            Licenses licenses = new Licenses();
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            string defaultDir = GetDefaultDirectory;
            if (string.IsNullOrEmpty(defaultDir))
                dialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            else
                dialog.SelectedPath = defaultDir;
            DialogResult result = dialog.ShowDialog();
            pathBox.Text = dialog.SelectedPath;
        }

        private void pathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(pathBox.Text))
            {
                IsGitEnabled.IsChecked = IsGitDirectory;
                dpStatus.Text = GetDataPackageJsonStatus;

            }
        }

        public string GetDefaultDirectory
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

        public bool IsGitDirectory
        {
            get
            {
                return Directory.Exists(Path.Combine(pathBox.Text, ".git"));
            }
        }

        public string GetDataPackageJsonStatus
        {
            get
            {
                if (IsExistDataPackageJson)
                    return "Exists";
                else
                    return "Create";
            }
        }

        public bool IsExistDataPackageJson
        {
            get
            {
                return File.Exists(Path.Combine(pathBox.Text, "DataPackage.json"));
            }
        }
    }
}
