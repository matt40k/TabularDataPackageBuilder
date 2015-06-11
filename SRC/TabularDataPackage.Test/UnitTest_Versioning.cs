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
            versioning.SetVersion("v.1.1");
            Assert.AreEqual(new Version("1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMinorIncrFromNull()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("0.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFromNull()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("1.0"), versioning.GetVersion);
        }


        [TestMethod]
        public void VersioningMinorIncrFrom20()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("2.0");
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("2.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFrom20()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("2.0");
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("3.0"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMinorIncrFrom32()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("3.2");
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("3.3"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFrom32()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("3.2");
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("4.2"), versioning.GetVersion);
        }


        [TestMethod]
        public void VersioningMinorIncrFrom1111()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("1.1.1.1");
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("1.2.1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFrom1111()
        {
            TabularDataPackage.Versioning versioning = new TabularDataPackage.Versioning();
            versioning.SetVersion("1.1.1.1");
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("2.1.1.1"), versioning.GetVersion);
        }
    }
}
