﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace TabularDataPackage
{
    public class LicenseJson
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly List<Licenses> _licenses;

        public LicenseJson()
        {
            logger.Log(LogLevel.Trace, "LicenseJson.LicenseJson()");
            _licenses = Deserial(ReadLicenseFile);
        }

        /// <summary>
        ///     Returns the License file name
        ///     Always returns License.json
        /// </summary>
        public string LicenseListFileName
        {
            get
            {
                logger.Log(LogLevel.Trace, "LicenseJson.LicenseListFileName");
                return "License.json";
            }
        }

        /// <summary>
        ///     Reads the license.json file from the file system
        /// </summary>
        public string ReadLicenseFile
        {
            get
            {
                logger.Log(LogLevel.Trace, "LicenseJson.ReadLicenseFile");
                if (File.Exists(LicenseListFileName))
                    return File.ReadAllText(LicenseListFileName);
                throw new Exception("License.json doesn't exist");
            }
        }

        /// <summary>
        ///     Returns the License list to external classes
        /// </summary>
        public List<Licenses> GetLicenses
        {
            get
            {
                logger.Log(LogLevel.Trace, "LicenseJson.GetLicenses");
                return _licenses;
            }
        }

        /// <summary>
        ///     Turns the license json string into a List of Licenses
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Licenses> Deserial(string json)
        {
            logger.Log(LogLevel.Trace, "LicenseJson.Deserial");
            return JsonConvert.DeserializeObject<List<Licenses>>(json);
            ;
        }

        /// <summary>
        ///     Looks up the license Title from the License list using the user
        ///     defined license Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Looks up the license Id from the License list using the user
        ///     defined license Name\Title
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetIdFromName(string name)
        {
            logger.Log(LogLevel.Trace, "LicenseJson.GetNameFromId");
            foreach (var license in _licenses)
            {
                if (license.License.Title == name)
                    return license.License.Id;
            }
            return null;
        }
    }
}