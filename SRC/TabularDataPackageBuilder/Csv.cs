﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NLog;

namespace TabularDataPackage
{
    /// <summary>
    /// 
    /// </summary>
    public class Csv
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private string filePath;

        /// <summary>
        ///     Returns the SHA1 checksum for the defined file
        /// </summary>
        /// <param name="file">file path</param>
        /// <returns>SHA1</returns>
        public string GetSHA1Hash
        {
            get
            {
                logger.Log(LogLevel.Trace, "Csv.GetSHA1Hash");
                StringBuilder formatted;
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var bs = new BufferedStream(fs))
                {
                    using (var sha1 = new SHA1Managed())
                    {
                        var hash = sha1.ComputeHash(bs);
                        formatted = new StringBuilder(2*hash.Length);
                        foreach (var b in hash)
                        {
                            formatted.AppendFormat("{0:X2}", b);
                        }
                    }
                }
                return formatted.ToString();
            }
        }

        /// <summary>
        ///     Determines a text file's encoding by analyzing its byte order mark (BOM).
        ///     Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filePath">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public bool GetIsUTF8
        {
            get
            {
                logger.Log(LogLevel.Trace, "Csv.GetIsUTF8");
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
        }

        /// <summary>
        ///     Returns the file size (bytes)
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>bytes</returns>
        public long GetFileSizeInBytes
        {
            get
            {
                logger.Log(LogLevel.Trace, "Csv.GetFileSizeInBytes");
                var _fileInfo = new FileInfo(filePath);
                return _fileInfo.Length;
            }
        }

        /// <summary>
        ///     Sets the CSV full file path to the CSV class
        /// </summary>
        public string Load
        {
            set
            {
                logger.Log(LogLevel.Trace, "Csv.Load");
                filePath = value;
            }
        }

        /// <summary>
        ///     Returns a list of CSV column by reading the first two lines.
        ///     The first license is the title of the column name and the second line is used
        ///     to determine the column type - using the DataPackage type
        /// </summary>
        public List<CsvColumn> GetCsvColumns
        {
            get
            {
                logger.Log(LogLevel.Trace, "Csv.GetCsvColumns");
                var _csvColumns = new List<CsvColumn>();
                string header;
                string body;

                using (var reader = new StreamReader(filePath))
                {
                    header = reader.ReadLine();
                    body = reader.ReadLine();
                }

                var headers = header.Split(',');
                var columns = body.Split(',');

                var cnt = headers.Length;
                var n = 0;
                while (n < cnt)
                {
                    try
                    {
                        var name = GetCleanName(headers[n]);
                        _csvColumns.Add(new CsvColumn {Name = name, Type = ConvertStringToType(columns[n])});
                    }
                    catch (Exception)
                    {
                    }
                    n++;
                }
                return _csvColumns;
            }
        }

        /// <summary>
        ///     Returns the File name (include extension) from the full (user defined)
        ///     file path
        /// </summary>
        public string GetFileName
        {
            get
            {
                logger.Log(LogLevel.Trace, "Csv.GetFileName");
                return Path.GetFileName(filePath);
            }
        }

        /// <summary>
        ///     Gets the file hash (prefix with hashing algorithms if MD5 isn't used)
        ///     As per the DataPackage spec - http://dataprotocols.org/data-packages/
        /// </summary>
        public string GetHash
        {
            get
            {
                logger.Log(LogLevel.Trace, "Csv.GetHash");
                return "sha1:" + GetSHA1Hash;
            }
        }

        /// <summary>
        ///     Returns a built resource for the DataPackage based on the user defined
        ///     csv file.
        /// </summary>
        public DataPackageResource GetFileResource
        {
            get
            {
                logger.Log(LogLevel.Trace, "Csv.GetFileResource");
                var resource = new DataPackageResource();
                resource.Path = GetFileName;
                resource.Hash = GetHash;
                resource.Bytes = GetFileSizeInBytes;
                var CsvColumns = GetCsvColumns;
                resource.Schema = new DataPackageResourceSchema();
                var fields = new List<DataPackageResourceSchemaField>();
                foreach (var column in CsvColumns)
                {
                    logger.Log(LogLevel.Trace, "Csv.GetFileResource - " + column.Name);
                    var field = new DataPackageResourceSchemaField();
                    field.Name = column.Name;
                    field.Type = column.Type;
                    fields.Add(field);
                }
                resource.Schema.Fields = fields;
                return resource;
            }
        }

        /// <summary>
        ///     Determins the DataPackage type of a (string) data value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertStringToType(string value)
        {
            logger.Log(LogLevel.Trace, "Csv.ConvertStringToType()");
            DateTime dateTimeResult;
            if (DateTime.TryParse(value, out dateTimeResult))
            {
                return "DateTime";
            }

            // First check the whole number types, because floating point types will always parse whole numbers
            // Start with the smallest types
            byte byteResult;
            if (byte.TryParse(value, out byteResult))
            {
                return "integer";
            }

            short shortResult;
            if (short.TryParse(value, out shortResult))
            {
                return "integer";
            }

            int intResult;
            if (int.TryParse(value, out intResult))
            {
                return "integer";
            }

            long longResult;
            if (long.TryParse(value, out longResult))
            {
                return "integer";
            }

            ulong ulongResult;
            if (ulong.TryParse(value, out ulongResult))
            {
                return "integer";
            }

            // No need to check the rest of the unsigned types, which will fit into the signed whole number types

            // Next check the floating point types
            float floatResult;
            if (float.TryParse(value, out floatResult))
            {
                return "number";
            }

            // It's not clear that there's anything that double.TryParse() and decimal.TryParse() will parse 
            // but which float.TryParse() won't
            double doubleResult;
            if (double.TryParse(value, out doubleResult))
            {
                return "number";
            }

            decimal decimalResult;
            if (decimal.TryParse(value, out decimalResult))
            {
                return "number";
            }

            // It's not a number, so it's either a bool, char or string
            bool boolResult;
            if (bool.TryParse(value, out boolResult))
            {
                return "boolean";
            }

            char charResult;
            if (char.TryParse(value, out charResult))
            {
                return "string";
            }

            return "string";
        }

        /// <summary>
        ///     Cleans the name (title) of the column as it could be encased in "
        /// </summary>
        /// <param name="unclean"></param>
        /// <returns></returns>
        public string GetCleanName(string unclean)
        {
            if ((unclean.Substring(0, 1) == "\"") && (unclean.Substring((unclean.Length - 1), 1) == "\""))
            {
                return unclean.Substring(1, (unclean.Length - 2));
            }
            return unclean;
        }
    }
}