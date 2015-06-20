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
            logger.Log(LogLevel.Trace, "UserInterface()");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            logger.Log(LogLevel.Trace, "UserInterface.buttonBrowse_Click()");
            var dialog = new FolderBrowserDialog();
            string defaultDir = GetDefaultDirectory;
            if (string.IsNullOrEmpty(defaultDir))
                dialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            else
                dialog.SelectedPath = defaultDir;
            DialogResult result = dialog.ShowDialog();
            pathBox.Text = dialog.SelectedPath;
        }

        /// <summary>
        /// The save logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            logger.Log(LogLevel.Trace, "UserInterface.buttonSave_Click()");
            _versioning.IncreaseMinorVersion();
            //_versioning.IncreaseMajorVersion();
            _versioning.SetNewUpdatedDate();
            foreach (CsvList csvFile in _csvList)
            {
                Csv _csv = new Csv();
                _csv.Load = Path.Combine(pathBox.Text,(csvFile.Filename + ".csv"));
                _dataPackage.Resources.Add(_csv.GetFileResource);
            }
            _dataPackages.Save(_dataPackage);
        }

        /// <summary>
        /// Enables or disables the user fields depending if a folder has been selected
        /// </summary>
        /// <param name="status"></param>
        private void enableDisable(bool status)
        {
            logger.Log(LogLevel.Trace, "UserInterface.enableDisable()");
            this.nameBox.IsEnabled = status;
            this.titleBox.IsEnabled = status;
            this.descriptionBox.IsEnabled = status;
            this.licenseBox.IsEnabled = status;
            this.versionBox.IsEnabled = status;
            this.lastUpdatedBox.IsEnabled = status;
            this.csvList.IsEnabled = status;
            this.buttonSave.IsEnabled = status;
        }
        

        /// <summary>
        /// Logic for when the path changes.
        /// 
        /// Checks the folder is a valid (ie it exists)
        /// If it is valid it:
        /// - enables the user fields depending if the folder is valid
        /// - sets the folder that the DataPackages class will use, which in turn loads the datapackage.json
        /// - sets the user fields properties to that of the datapackage.json file
        /// 
        /// If the folder isn't valid, it clears the user fields and disables them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            logger.Log(LogLevel.Trace, "UserInterface.pathBox_TextChanged()");
            if (Directory.Exists(pathBox.Text))
            {
                enableDisable(true);
                IsGitEnabled.IsChecked = IsGitDirectory;
                dpStatus.Text = GetDataPackageJsonStatus;
                _dataPackages.ProjectDirectory = pathBox.Text;
                LoadPropertiesFromPackage();
            }
            else
            {
                enableDisable(false);
                clearSettings();
            }
        }

        /// <summary>
        /// Clears all the user fields
        /// </summary>
        private void clearSettings()
        {
            logger.Log(LogLevel.Trace, "UserInterface.clearSettings()");
            this.nameBox.Text = "";
            this.titleBox.Text = "";
            this.descriptionBox.Text = "";
            this.licenseBox.Text = "";
            this.versionBox.Text = "";
            this.lastUpdatedBox.Text = "";
            _csvList.Clear();
            this.csvList.ItemsSource = null;
        }

        /// <summary>
        /// Loads the properties from the datapackage.json file
        /// </summary>
        private void LoadPropertiesFromPackage()
        {
            logger.Log(LogLevel.Trace, "UserInterface.LoadPropertiesFromPackage()");
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
                    _csvList.Add(new CsvList() { Selected = true, Filename = Path.GetFileNameWithoutExtension(csvFile), InPackage = _dataPackages.InPackage(_dataPackage, csvFile) });
                }
                this.csvList.ItemsSource = _csvList;
            }
            else
                clearSettings();
        }

        /// <summary>
        /// Returns the default starting folder for the browse options, default is the GitHub folder 
        /// within My Documents, if this doesn't exist, we don't default. Stranges things can happen 
        /// if we default to My Documents if Folder Redirection is play.
        /// </summary>
        public string GetDefaultDirectory
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.GetDefaultDirectory");
                string githubDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GitHub");
                if (Directory.Exists(githubDir))
                    return githubDir;
                else
                    return null;
            }
        }

        /// <summary>
        /// Git directories will have a .git folder at the root of the repository. However,
        /// this doesn't take into account us looking at a subfolder of a repository, after all
        /// where would it end if we did?
        /// </summary>
        public bool IsGitDirectory
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.IsGitDirectory");
                return Directory.Exists(Path.Combine(pathBox.Text, ".git"));
            }
        }

        /// <summary>
        /// Checks to see if a DataPackage.json already exists in the folder...
        /// and returns a friendly user instruction.
        /// </summary>
        public string GetDataPackageJsonStatus
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.GetDataPackageJsonStatus");
                if (IsExistDataPackageJson)
                    return "Exists";
                else
                    return "Create";
            }
        }

        /// <summary>
        /// Returns the full file path for the DataPackage.json by combining DataPackage.json to
        /// the user defined folder
        /// </summary>
        public string DataPackageJsonFilePath
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.DataPackageJsonFilePath");
                return Path.Combine(pathBox.Text, "DataPackage.json");
            }
        }

        /// <summary>
        /// Checks to see if a DataPackage.json already exists in the folder
        /// </summary>
        public bool IsExistDataPackageJson
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.IsExistDataPackageJson");
                return File.Exists(DataPackageJsonFilePath);
            }
        }

        /// <summary>
        /// Returns an array of the CSV files within the user defined folder
        /// </summary>
        public string[] CsvFiles
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.CsvFiles");
                return Directory.GetFiles(pathBox.Text, "*.csv", SearchOption.TopDirectoryOnly);
            }
        }
    }
}
