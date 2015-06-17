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

        public DateTime GetLastUpdated
        {
            get
            {
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

        public void SetNewUpdatedDate()
        {
            _lastUpdated = DateTime.Now;
        }

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
