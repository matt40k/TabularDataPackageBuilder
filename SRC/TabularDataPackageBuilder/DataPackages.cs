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

        public string DataPackageFileName
        {
            get
            {
                logger.Log(LogLevel.Trace, "DataPackages.DataPackageFileName");
                return "DataPackage.json";
            }
        }

        public DataPackage Load
        {
            get
            {
                logger.Log(LogLevel.Trace, "DataPackages.Load");
                return Deserial(File.ReadAllText(Path.Combine(ProjectDirectory, DataPackageFileName)));
            }
        }

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

        public static DataPackage Deserial(string json)
        {
            logger.Log(LogLevel.Trace, "DataPackages.Deserial");
            var dataPackage = JsonConvert.DeserializeObject<DataPackage>(json);
            return dataPackage;
        }

        public static string Serial(DataPackage dataPackage)
        {
            logger.Log(LogLevel.Trace, "DataPackages.Serial");
            var json = JsonConvert.SerializeObject(dataPackage);
            return json;
        }

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
