using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TabularDataPackage;

namespace TabularDataPackage.Test
{
    /// <summary>
    /// Summary description for UnitTest_Csv
    /// </summary>
    [TestClass]
    public class UnitTest_Csv
    {
        public UnitTest_Csv()
        {

        }

        [TestMethod]
        public void Csv()
        {
            TabularDataPackage.Csv versioning = new TabularDataPackage.Csv();
            Assert.AreEqual(0,0);
        }

    }
}
