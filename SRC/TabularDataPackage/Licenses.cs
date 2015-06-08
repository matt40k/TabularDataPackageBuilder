using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace TabularDataPackage
{
    public class Licenses
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private License _licenses;

        public Licenses()
        {
            _licenses = Deserial(ReadLicenseFile);
        }

        public string LicenseListFileName
        {
            get
            {
                return "License.json";
            }
        }

        public string ReadLicenseFile
        {
            get
            {
                return File.ReadAllText(LicenseListFileName);
            }
        }

        public static License Deserial(string json)
        {
            var license = JsonConvert.DeserializeObject<License>(json);
            return license;
        }
    }
}
