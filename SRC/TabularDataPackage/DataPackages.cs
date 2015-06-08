using System;
using System.IO;
using Newtonsoft.Json;

namespace TabularDataPackage
{
    public class DataPackages
    {
        private string _ProjectDirectory;

        private string ProjectDirectory
        {
            get
            {
                return _ProjectDirectory;
            }
            set
            {
                if (Directory.Exists(value))
                    _ProjectDirectory = value;
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
    }
}
