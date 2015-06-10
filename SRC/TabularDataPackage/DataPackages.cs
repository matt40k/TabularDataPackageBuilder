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
                return _ProjectDirectory;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    _ProjectDirectory = value;
                }
            }
        }

        public DataPackages()
        {
            
        }

        public string DataPackageFileName
        {
            get
            {
                return "DataPackage.json";
            }
        }

        public DataPackage Load
        {
            get
            {
                return Deserial(File.ReadAllText(Path.Combine(ProjectDirectory, DataPackageFileName)));
            }
        }

        public static DataPackage Deserial(string json)
        {
            var dataPackage = JsonConvert.DeserializeObject<DataPackage>(json);
            return dataPackage;
        }

        public bool InPackage(DataPackage dataPackage, string physicalName)
        {
            foreach (DataPackageResource resource in dataPackage.Resources)
            {
                if (physicalName == resource.Path)
                    return true;
            }
            return false;
        }
    }
}
