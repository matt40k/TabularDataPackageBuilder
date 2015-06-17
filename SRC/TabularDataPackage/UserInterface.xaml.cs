using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using NLog;

namespace TabularDataPackage
{
    /// <summary>
    /// Interaction logic for UserInterface.xaml
    /// </summary>
    public partial class UserInterface : Window
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly FolderBrowserDialog openFileDialog;
        private DataPackages _dataPackages;
        private DataPackage _dataPackage;
        private LicenseJson licenses;
        private Versioning _versioning;
        private List<CsvList> _csvList = new List<CsvList>();

        public UserInterface()
        {
            InitializeComponent();

            openFileDialog = new FolderBrowserDialog();
            licenses = new LicenseJson();
            _dataPackages = new DataPackages();
            _versioning = new Versioning();

            foreach (var license in licenses.GetLicenses)
            {
                this.licenseBox.Items.Add(license.License.Title);
            }
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

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void enableDisable(bool status)
        {
            nameBox.IsEnabled = status;
            titleBox.IsEnabled = status;
            descriptionBox.IsEnabled = status;
            licenseBox.IsEnabled = status;
            versionBox.IsEnabled = status;
            lastUpdatedBox.IsEnabled = status;
            csvList.IsEnabled = status;
        }

        private void pathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(pathBox.Text))
            {
                enableDisable(true);
                IsGitEnabled.IsChecked = IsGitDirectory;
                dpStatus.Text = GetDataPackageJsonStatus;
                _dataPackages.ProjectDirectory = pathBox.Text;
                LoadPropertiesFromPackage();
                this.buttonSave.IsEnabled = true;
            }
            else
            {
                enableDisable(false);
                clearSettings();
            }
        }

        private void clearSettings()
        {
            this.nameBox.Text = "";
            this.titleBox.Text = "";
            this.descriptionBox.Text = "";
            this.licenseBox.Text = "";
            this.versionBox.Text = "";
            this.lastUpdatedBox.Text = "";
            this.csvList.Items.Clear();
        }

        private void LoadPropertiesFromPackage()
        {
            if (IsExistDataPackageJson)
            {
                _dataPackage = _dataPackages.Load;
                this.nameBox.Text = _dataPackage.Name;
                this.titleBox.Text = _dataPackage.Title;
                this.descriptionBox.Text = _dataPackage.Description;
                this.licenseBox.SelectedValue = licenses.GetNameFromId(_dataPackage.License);
                _versioning.DataPackageJsonFilePath = DataPackageJsonFilePath;
                _versioning.SetVersion(_dataPackage.Version);
                _versioning.SetLastUpdated(_dataPackage.LastUpdated);
                this.versionBox.Text = _versioning.GetVersion.ToString();
                this.lastUpdatedBox.Text = _versioning.GetLastUpdated.ToString();

                foreach (string csvFile in CsvFiles)
                {
                    _csvList.Add(new CsvList() { Selected = false, Filename = Path.GetFileNameWithoutExtension(csvFile), InPackage = _dataPackages.InPackage(_dataPackage, csvFile) });
                }
                this.csvList.ItemsSource = _csvList;
            }
            else
                clearSettings();
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

        public string DataPackageJsonFilePath
        {
            get
            {
                return Path.Combine(pathBox.Text, "DataPackage.json");
            }
        }

        public bool IsExistDataPackageJson
        {
            get
            {
                return File.Exists(DataPackageJsonFilePath);
            }
        }

        public string[] CsvFiles
        {
            get
            {
                return Directory.GetFiles(pathBox.Text, "*.csv", SearchOption.TopDirectoryOnly);
            }
        }
    }
}
