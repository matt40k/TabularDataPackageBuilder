using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using NLog;

namespace TabularDataPackage
{
    /// <summary>
    ///     Interaction logic for UserInterface.xaml
    /// </summary>
    public partial class UserInterface : Window
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly List<CsvList> _csvList = new List<CsvList>();
        private readonly DataPackages _dataPackages;
        private readonly LicenseJson _licenses;
        private readonly Versioning _versioning;
        private readonly FolderBrowserDialog openFileDialog;
        private DataPackage _dataPackage;

        public UserInterface()
        {
            logger.Log(LogLevel.Trace, "UserInterface()");
            InitializeComponent();

            openFileDialog = new FolderBrowserDialog();
            _licenses = new LicenseJson();
            _dataPackages = new DataPackages();
            _versioning = new Versioning();

            foreach (var _license in _licenses.GetLicenses)
            {
                licenseBox.Items.Add(_license.License.Title);
            }
        }

        /// <summary>
        ///     Returns the default starting folder for the browse options, default is the GitHub folder
        ///     within My Documents, if this doesn't exist, we don't default. Stranges things can happen
        ///     if we default to My Documents if Folder Redirection is play.
        /// </summary>
        public string GetDefaultDirectory
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.GetDefaultDirectory");
                var githubDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GitHub");
                if (Directory.Exists(githubDir))
                    return githubDir;
                return null;
            }
        }

        /// <summary>
        ///     Git directories will have a .git folder at the root of the repository. However,
        ///     this doesn't take into account us looking at a subfolder of a repository, after all
        ///     where would it end if we did?
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
        ///     Checks to see if a DataPackage.json already exists in the folder...
        ///     and returns a friendly user instruction.
        /// </summary>
        public string GetDataPackageJsonStatus
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.GetDataPackageJsonStatus");
                if (IsExistDataPackageJson)
                    return "Exists";
                return "Create";
            }
        }

        /// <summary>
        ///     Returns the full file path for the DataPackage.json by combining DataPackage.json to
        ///     the user defined folder
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
        ///     Checks to see if a DataPackage.json already exists in the folder
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
        ///     Returns a string array of the CSV files within the user defined folder
        /// </summary>
        public string[] CsvFiles
        {
            get
            {
                logger.Log(LogLevel.Trace, "UserInterface.CsvFiles");
                return Directory.GetFiles(pathBox.Text, "*.csv", SearchOption.TopDirectoryOnly);
            }
        }

        /// <summary>
        ///     Opens a browse menu so the user can select the project folder
        ///     This folder will house the DataPackage.json file (or will once they hit save)
        ///     It most contain the CSV files that will be used
        ///     Defaults the starting position to the GitHub directory within
        ///     My Documents, if it exists, otherwise it doesn't default it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            logger.Log(LogLevel.Trace, "UserInterface.buttonBrowse_Click()");
            var dialog = new FolderBrowserDialog();
            var defaultDir = GetDefaultDirectory;
            if (string.IsNullOrEmpty(defaultDir))
                dialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            else
                dialog.SelectedPath = defaultDir;
            var result = dialog.ShowDialog();
            pathBox.Text = dialog.SelectedPath;
        }

        /// <summary>
        ///     The save logic
        ///     It increases the version number
        ///     Creates a DataPackage resource for each CSV file in the directory that
        ///     is selected for inclusion
        ///     Writes the DataPackage object out to the file system as DataPackage.json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            logger.Log(LogLevel.Trace, "UserInterface.buttonSave_Click()");

            _versioning.IncreaseMinorVersion();
            //_versioning.IncreaseMajorVersion();
            _versioning.SetNewUpdatedDate();
            // TODO deal with the user input changing the last updated and the version no

            // Clear all the csv resources from the DataPackage
            _dataPackage.Resources.Clear();
            // Add any selected CSV resources to the DataPackage
            foreach (var csvFile in _csvList.FindAll(x => x.Selected))
            {
                var _csv = new Csv();
                _csv.Load = Path.Combine(pathBox.Text, (csvFile.Filename + ".csv"));
                _dataPackage.Resources.Add(_csv.GetFileResource);
            }
            SetPropertiesToPackage();
            _dataPackages.Save(_dataPackage);
        }

        /// <summary>
        ///     Enables or disables the user fields depending if a folder has been selected
        /// </summary>
        /// <param name="status"></param>
        private void enableDisable(bool status)
        {
            logger.Log(LogLevel.Trace, "UserInterface.enableDisable()");
            nameBox.IsEnabled = status;
            titleBox.IsEnabled = status;
            descriptionBox.IsEnabled = status;
            licenseBox.IsEnabled = status;
            versionBox.IsEnabled = status;
            lastUpdatedBox.IsEnabled = status;
            csvList.IsEnabled = status;
            buttonSave.IsEnabled = status;
        }

        /// <summary>
        ///     Logic for when the path changes.
        ///     Checks the folder is a valid (ie it exists)
        ///     If it is valid it:
        ///     - enables the user fields depending if the folder is valid
        ///     - sets the folder that the DataPackages class will use, which in turn loads the datapackage.json
        ///     - sets the user fields properties to that of the datapackage.json file
        ///     If the folder isn't valid, it clears the user fields and disables them
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
        ///     Clears all the user fields
        /// </summary>
        private void clearSettings()
        {
            logger.Log(LogLevel.Trace, "UserInterface.clearSettings()");
            nameBox.Text = "";
            titleBox.Text = "";
            descriptionBox.Text = "";
            licenseBox.Text = "";
            versionBox.Text = "";
            lastUpdatedBox.Text = "";
            _csvList.Clear();
            csvList.ItemsSource = null;
        }

        /// <summary>
        ///     Loads the properties from the datapackage.json file
        /// </summary>
        private void LoadPropertiesFromPackage()
        {
            logger.Log(LogLevel.Trace, "UserInterface.LoadPropertiesFromPackage()");
            if (IsExistDataPackageJson)
            {
                _dataPackage = _dataPackages.Load;
                nameBox.Text = _dataPackage.Name;
                titleBox.Text = _dataPackage.Title;
                descriptionBox.Text = _dataPackage.Description;
                licenseBox.SelectedValue = _licenses.GetNameFromId(_dataPackage.License);
                _versioning.DataPackageJsonFilePath = DataPackageJsonFilePath;
                _versioning.SetVersion(_dataPackage.Version);
                _versioning.SetLastUpdated(_dataPackage.LastUpdated);
                versionBox.Text = _versioning.GetVersion.ToString();
                lastUpdatedBox.Text = _versioning.GetLastUpdated.ToString();

                foreach (var csvFile in CsvFiles)
                {
                    _csvList.Add(new CsvList
                    {
                        Selected = true,
                        Filename = Path.GetFileNameWithoutExtension(csvFile),
                        InPackage = _dataPackages.InPackage(_dataPackage, csvFile)
                    });
                }
                csvList.ItemsSource = _csvList;
            }
            else
                clearSettings();
        }

        /// <summary>
        ///     Set the user defined fields to the DataPackage object
        /// </summary>
        private void SetPropertiesToPackage()
        {
            logger.Log(LogLevel.Trace, "UserInterface.SetPropertiesToPackage()");
            if (!string.IsNullOrEmpty(nameBox.Text))
                _dataPackage.Name = nameBox.Text;
            if (!string.IsNullOrEmpty(titleBox.Text))
                _dataPackage.Title = titleBox.Text;
            if (!string.IsNullOrEmpty(descriptionBox.Text))
                _dataPackage.Description = descriptionBox.Text;

            string licenseId = null;
            try
            {
                var licenseName = (string) licenseBox.SelectedValue;
                licenseId = _licenses.GetIdFromName(licenseName);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }

            if (!string.IsNullOrEmpty(licenseId))
                _dataPackage.License = licenseId;
            _dataPackage.Version = _versioning.GetVersion.ToString();
            _dataPackage.LastUpdated = _versioning.GetLastUpdated.ToString();
        }
    }
}