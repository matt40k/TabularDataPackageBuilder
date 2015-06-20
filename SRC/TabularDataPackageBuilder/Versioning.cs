using System;
using System.IO;
using NLog;

namespace TabularDataPackage
{
    public class Versioning
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Version _version;
        private DateTime _lastUpdated;
        public string DataPackageJsonFilePath { get; set; }

        /// <summary>
        /// Returns the version to external classes, if the version is currently 
        /// null, it returns 0.1
        /// </summary>
        public Version GetVersion
        {
            get
            {
                logger.Log(LogLevel.Trace, "Versioning.GetVersion");
                if (_version == null)
                    return new Version("0.1");
                return _version;
            }
        }

        /// <summary>
        /// Returns the Last Updated (datetime stamp) for external licenses from
        /// the DataPackage, if it is null, then it uses the last write time of 
        /// the DataPackage.json file. If that does not exist, it uses the current
        /// DateTime.
        /// </summary>
        public DateTime GetLastUpdated
        {
            get
            {
                logger.Log(LogLevel.Trace, "Versioning.GetLastUpdated");
                if (_lastUpdated == new DateTime())
                {
                    if (File.Exists(DataPackageJsonFilePath))
                    {
                        return File.GetLastWriteTime(DataPackageJsonFilePath);
                    }
                    else
                    {
                        return DateTime.Now;
                    }
                }
                return _lastUpdated;
            }
        }

        /// <summary>
        /// Sets the version number based on the user input.
        /// 
        /// It does basic cleanup before parsing - it removes a 'v' prefix. 
        /// If nothing is past, it sets it as 0.1.
        /// 
        /// Returns false if it fails to parse it.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool SetVersion(string version)
        {
            logger.Log(LogLevel.Trace, "Versioning.SetVersion");

            if (string.IsNullOrEmpty(version))
            {
                _version = new Version("0.1");
                return true;
            }
            Version ver = null;
            
            version = version.ToLower();
            version = version.Replace("v.","");
            version = version.Replace("v","");
            version = version.Replace(" ","");
            
            bool result = Version.TryParse(version, out ver);
            _version = ver;
            return result;
        }

        /// <summary>
        /// Sets the LastUpdated based on the user input.
        /// Returns false if it fails to parse it.
        /// </summary>
        /// <param name="LastUpdated"></param>
        /// <returns></returns>
        public bool SetLastUpdated(string LastUpdated)
        {
            logger.Log(LogLevel.Trace, "Versioning.SetLastUpdated");
            if (string.IsNullOrEmpty(LastUpdated))
                return false;
            DateTime dateTime;
            bool result = DateTime.TryParse(LastUpdated, out dateTime);
            if (result)
            {
                _lastUpdated = dateTime;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the LastUpdated date with te current datetime
        /// </summary>
        public void SetNewUpdatedDate()
        {
            logger.Log(LogLevel.Trace, "Versioning.SetNewUpdatedDate");
            _lastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Increases the version by one minor version
        /// 
        /// For example, 1.0 will become 1.1
        /// </summary>
        public void IncreaseMinorVersion()
        {
            logger.Log(LogLevel.Trace, "Versioning.IncreaseMinorVersion");
            if (_version == null)
                _version = new Version("0.1");
            else
            {
                if (_version.Revision > 0)
                    _version = new Version(_version.Major, (_version.Minor + 1), _version.Build, _version.Revision);
                else
                {
                    if (_version.Build > 0)
                        _version = new Version(_version.Major, (_version.Minor + 1), _version.Build);
                    else
                        _version = new Version(_version.Major, (_version.Minor + 1));
                }
            }
        }


        /// <summary>
        /// Increases the version by one major version
        /// 
        /// For example, 1.0 will become 2.0
        /// </summary>
        public void IncreaseMajorVersion()
        {
            logger.Log(LogLevel.Trace, "Versioning.IncreaseMajorVersion");
            if (_version == null)
                _version = new Version("1.0");
            else
            {
                if (_version.Revision > 0)
                    _version = new Version((_version.Major +1), _version.Minor, _version.Build, _version.Revision);
                else
                {
                    if (_version.Build > 0)
                        _version = new Version((_version.Major + 1), _version.Minor, _version.Build);
                    else
                        _version = new Version((_version.Major + 1), _version.Minor);
                }
            }
        }
    }
}
