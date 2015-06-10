using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using NLog;

namespace TabularDataPackage
{
    public class LicenseJson
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private List<Licenses> _licenses;

        public LicenseJson()
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
                if (File.Exists(LicenseListFileName))
                    return File.ReadAllText(LicenseListFileName);
                else
                    throw new Exception("License.json doesn't exist");
            }
        }

        public static List<Licenses> Deserial(string json)
        {
            return JsonConvert.DeserializeObject<List<Licenses>>(json); ;
        }

        public List<Licenses> GetLicenses
        {
            get
            {
                return _licenses;
            }
        }

        public string GetNameFromId(string Id)
        {
            foreach (var license in _licenses)
            {
                if (license.License.Id == Id)
                    return license.License.Title;
            }
            return null;
        }
    }
}
