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
            logger.Log(LogLevel.Trace, "LicenseJson.LicenseJson()");
            _licenses = Deserial(ReadLicenseFile);
        }

        public string LicenseListFileName
        {
            get
            {
                logger.Log(LogLevel.Trace, "LicenseJson.LicenseListFileName");
                return "License.json";
            }
        }

        public string ReadLicenseFile
        {
            get
            {
                logger.Log(LogLevel.Trace, "LicenseJson.ReadLicenseFile");
                if (File.Exists(LicenseListFileName))
                    return File.ReadAllText(LicenseListFileName);
                else
                    throw new Exception("License.json doesn't exist");
            }
        }

        public static List<Licenses> Deserial(string json)
        {
            logger.Log(LogLevel.Trace, "LicenseJson.Deserial");
            return JsonConvert.DeserializeObject<List<Licenses>>(json); ;
        }

        public List<Licenses> GetLicenses
        {
            get
            {
                logger.Log(LogLevel.Trace, "LicenseJson.GetLicenses");
                return _licenses;
            }
        }

        public string GetNameFromId(string Id)
        {
            logger.Log(LogLevel.Trace, "LicenseJson.GetNameFromId");
            foreach (var license in _licenses)
            {
                if (license.License.Id == Id)
                    return license.License.Title;
            }
            return null;
        }
    }
}
