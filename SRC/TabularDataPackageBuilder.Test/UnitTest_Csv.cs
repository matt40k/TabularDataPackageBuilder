using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TabularDataPackage.Test
{
    /// <summary>
    ///     Summary description for UnitTest_Csv
    /// </summary>
    [TestClass]
    public class UnitTest_Csv
    {
        [TestMethod]
        public void CsvValidSHA1Hash()
        {
            var _csv = new Csv();
            var contents = "This is a test file. Please delete me.";
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, contents);
            _csv.Load = tempFile;
            var hash = _csv.GetSHA1Hash;
            File.Delete(tempFile);
            Assert.AreEqual("CC5872242874C1D33B6B41F37279321D6959F34C", hash);
        }

        [TestMethod]
        public void CsvInvalidSHA1Hash()
        {
            var _csv = new Csv();
            var contents = "This is a test file. Please delete me.";
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, contents);
            _csv.Load = tempFile;
            var hash = _csv.GetSHA1Hash;
            File.Delete(tempFile);
            Assert.AreNotEqual("NotTheHash", hash);
        }

        [TestMethod]
        public void CsvIsUTF8()
        {
            var _csv = new Csv();
            var contents = "This is a test file. Please delete me.";
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, contents, Encoding.UTF8);
            _csv.Load = tempFile;
            var isUTF8 = _csv.GetIsUTF8;
            File.Delete(tempFile);
            Assert.AreEqual(true, isUTF8);
        }

        [TestMethod]
        public void CsvIsUnicode()
        {
            var _csv = new Csv();
            var contents = "This is a test file. Please delete me.";
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, contents, Encoding.Unicode);
            _csv.Load = tempFile;
            var isUTF8 = _csv.GetIsUTF8;
            File.Delete(tempFile);
            Assert.AreNotEqual(true, isUTF8);
        }

        [TestMethod]
        public void CsvGetCleanNameWithOneQuote()
        {
            var _csv = new Csv();
            var cleaned = _csv.GetCleanName("\"Title");
            Assert.AreEqual("\"Title", cleaned);
        }

        [TestMethod]
        public void CsvGetCleanNameWithTwoQuote()
        {
            var _csv = new Csv();
            var cleaned = _csv.GetCleanName("\"Title\"");
            Assert.AreEqual("Title", cleaned);
        }

        [TestMethod]
        public void CsvGetCleanNameWithNoQuote()
        {
            var _csv = new Csv();
            var cleaned = _csv.GetCleanName("Title");
            Assert.AreEqual("Title", cleaned);
        }
    }
}