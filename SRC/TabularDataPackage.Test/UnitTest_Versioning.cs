using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TabularDataPackage;

namespace TabularDataPackage.Test
{
    [TestClass]
    public class UnitTest_Versioning
    {
        [TestMethod]
        public void VersioningIsNullReturn01()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            Assert.AreEqual(new Version("0.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningIs11()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("1.1");
            Assert.AreEqual(new Version("1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningIsv11()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("v1.1");
            Assert.AreEqual(new Version("1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningIsvdot11()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("1.1");
            Assert.AreEqual(new Version("v.1.1"), versioning.GetVersion);
        }


    }
}
