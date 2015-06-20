using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace TabularDataPackage
{
    public class DataPackages
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private string _ProjectDirectory;

        /// <summary>
        /// GET
        /// Returns the user defined directory that is used.
        /// 
        /// Set
        /// Checks the folder exists and if it does sets it
        /// </summary>
        public string ProjectDirectory
        {
            get
            {
                logger.Log(LogLevel.Trace, "DataPackages.ProjectDirectory");
                return _ProjectDirectory;
            }
            set
            {
                logger.Log(LogLevel.Trace, "DataPackages.ProjectDirectory");
                if (Directory.Exists(value))
                {
                    _ProjectDirectory = value;
                }
            }
        }

        public DataPackages()
        {
            logger.Log(LogLevel.Trace, "DataPackages.DataPackages()");
        }

        /// <summary>
        /// Returns the DataPackage filename
        /// Always returns DataPackage.json
        /// </summary>
        public string DataPackageFileName
        {
            get
            {
                logger.Log(LogLevel.Trace, "DataPackages.DataPackageFileName");
                return "DataPackage.json";
            }
        }

        /// <summary>
        /// Loads the DataPackage.json file
        /// </summary>
        public DataPackage Load
        {
            get
            {
                logger.Log(LogLevel.Trace, "DataPackages.Load");
                return Deserial(File.ReadAllText(Path.Combine(ProjectDirectory, DataPackageFileName)));
            }
        }

        /// <summary>
        /// Saves the DataPackage to the file system
        /// </summary>
        /// <param name="dataPackage"></param>
        public void Save(DataPackage dataPackage)
        {
            logger.Log(LogLevel.Trace, "DataPackages.Save");
            try
            {
                File.WriteAllText((Path.Combine(ProjectDirectory, DataPackageFileName)), Serial(dataPackage));
            }
            catch (IOException ioException)
            {
                logger.Error(ioException);
            }
        }

        /// <summary>
        /// Turns the json string into a DataPackage object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataPackage Deserial(string json)
        {
            logger.Log(LogLevel.Trace, "DataPackages.Deserial");
            var dataPackage = JsonConvert.DeserializeObject<DataPackage>(json);
            return dataPackage;
        }

        /// <summary>
        /// Turns the DataPackage into a json string
        /// </summary>
        /// <param name="dataPackage"></param>
        /// <returns></returns>
        public static string Serial(DataPackage dataPackage)
        {
            logger.Log(LogLevel.Trace, "DataPackages.Serial");
            var json = JsonConvert.SerializeObject(dataPackage);
            return json;
        }

        /// <summary>
        /// Checks if the file in the folder exist (that has been specificed) exists
        /// in the DataPackage already
        /// </summary>
        /// <param name="dataPackage"></param>
        /// <param name="physicalName"></param>
        /// <returns></returns>
        public bool InPackage(DataPackage dataPackage, string physicalName)
        {
            logger.Log(LogLevel.Trace, "DataPackages.InPackage");
            foreach (DataPackageResource resource in dataPackage.Resources)
            {
                if (Path.GetFileName(physicalName) == resource.Path)
                    return true;
            }
            return false;
        }
    }
}
