using System;
using System.IO;
using Ionic.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zip.Tests
{
    /// <summary>
    /// Class for testing Zip module methods
    /// </summary>
    [TestClass]
    public class ZipMethodsTests
    {
        /// <summary>
        /// Test method for non-existing file in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnFailureIfFileDoesNotExist()
        {
            Assert.AreEqual(false, Methods.ZipFiles("assert", new[] {"Nonexisting or invalid project/solution file"}));
        }
        /// <summary>
        /// Test method for empty file path in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShoudReturnFailureIfFilePathIsEmpty()
        {
            Assert.AreEqual(false, Methods.ZipFiles("assert", new[] {""}));
        }
        /// <summary>
        /// Test method for successful zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfParametersAreValid()
        {
            Assert.AreEqual(true, Methods.ZipFiles("assert", new [] {"testZipFile.txt"}, ".."));
        }
        /// <summary>
        /// Test method for unspecified zip path in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfZipPathIsNotSpecified()
        {
            Assert.AreEqual(true, Methods.ZipFiles("assert", new [] {"testZipFile.txt"}));
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid zip path")]
        public void ZipFilesShouldThrowWhenZipPathIsNotValid()
        {
            Methods.ZipFiles("exception", new[] { "invalid zip path" });
        }
    }
}
