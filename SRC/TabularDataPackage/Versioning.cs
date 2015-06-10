using System;

namespace TabularDataPackage
{
    public class Versioning
    {
        private Version _version;

        public Version GetVersion
        {
            get
            {
                if (_version == null)
                    return new Version("0.1");
                return _version;
            }
        }

        public bool SetVersion(string version)
        {
            Version ver = null;
            bool result = Version.TryParse(version, out ver);
            _version = ver;
            return result;
        }
    }
}
