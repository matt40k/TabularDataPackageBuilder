using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TabularDataPackage.Test
{
    /// <summary>
    ///     Summary description for UnitTest_Versioning
    /// </summary>
    [TestClass]
    public class UnitTest_Versioning
    {
        [TestMethod]
        public void VersioningIsNullReturn01()
        {
            var versioning = new Versioning();
            Assert.AreEqual(new Version("0.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningIs11()
        {
            var versioning = new Versioning();
            versioning.SetVersion("1.1");
            Assert.AreEqual(new Version("1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningIsv11()
        {
            var versioning = new Versioning();
            versioning.SetVersion("v1.1");
            Assert.AreEqual(new Version("1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningIsvdot11()
        {
            var versioning = new Versioning();
            versioning.SetVersion("v.1.1");
            Assert.AreEqual(new Version("1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMinorIncrFromNull()
        {
            var versioning = new Versioning();
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("0.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFromNull()
        {
            var versioning = new Versioning();
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("1.0"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMinorIncrFrom20()
        {
            var versioning = new Versioning();
            versioning.SetVersion("2.0");
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("2.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFrom20()
        {
            var versioning = new Versioning();
            versioning.SetVersion("2.0");
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("3.0"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMinorIncrFrom32()
        {
            var versioning = new Versioning();
            versioning.SetVersion("3.2");
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("3.3"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFrom32()
        {
            var versioning = new Versioning();
            versioning.SetVersion("3.2");
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("4.2"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMinorIncrFrom1111()
        {
            var versioning = new Versioning();
            versioning.SetVersion("1.1.1.1");
            versioning.IncreaseMinorVersion();
            Assert.AreEqual(new Version("1.2.1.1"), versioning.GetVersion);
        }

        [TestMethod]
        public void VersioningMajorIncrFrom1111()
        {
            var versioning = new Versioning();
            versioning.SetVersion("1.1.1.1");
            versioning.IncreaseMajorVersion();
            Assert.AreEqual(new Version("2.1.1.1"), versioning.GetVersion);
        }
    }
}