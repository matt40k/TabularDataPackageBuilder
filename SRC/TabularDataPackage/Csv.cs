using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TabularDataPackage
{
    /// <summary>
    /// 
    /// </summary>
    public class Csv
    {
        /// <summary>
        /// Returns the SHA1 checksum for the defined file
        /// </summary>
        /// <param name="file">file path</param>
        /// <returns>SHA1</returns>
        public string GetSHA1Hash(string file)
        {
            StringBuilder formatted;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bs);
                    formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }
                }
            }
            return formatted.ToString();
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filePath">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public bool GetIsUTF8(string filePath)
        {
            // Source: http://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return true;
            return false;
        }

        /// <summary>
        /// Returns the file size (bytes)
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>bytes</returns>
        public long GetFileSizeInBytes(string filePath)
        {
            FileInfo _fileInfo = new FileInfo(filePath);
            return _fileInfo.Length;
        }

        public bool Load
        {
            get
            {
                //DataTable dbtable = CSVReader.ReadCSVFile(filename, true);
                return true;
            }
        }
    }
}
