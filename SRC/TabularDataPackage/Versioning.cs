using System;
using NLog;

namespace TabularDataPackage
{
    public class Versioning
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Version _version;

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

        public bool SetVersion(string version)
        {
            logger.Log(LogLevel.Trace, "Versioning.SetVersion");
            Version ver = null;
            bool result = Version.TryParse(version, out ver);
            _version = ver;
            return result;
        }
    }
}
