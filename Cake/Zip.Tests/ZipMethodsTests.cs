using System;
using System.IO;
using Ionic.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zip.Tests
{
    [TestClass]
    public class ZipMethodsTests
    {
        [TestMethod]
        public void ZipFilesShouldReturnFailureIfFileDoesNotExist()
        {
            Assert.AreEqual(false, Methods.ZipFiles("assert", new[] {"Nonexisting or invalid project/solution file"}));
        }

        [TestMethod]
        public void ZipFilesShoudReturnFailureIfFilePathIsEmpty()
        {
            Assert.AreEqual(false, Methods.ZipFiles("assert", new[] {""}));
        }

        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfParametersAreValid()
        {
            Assert.AreEqual(true, Methods.ZipFiles("assert", new [] {"testZipFile.txt"}, ".."));
        }

        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfZipPathIsNotSpecified()
        {
            Assert.AreEqual(true, Methods.ZipFiles("assert", new [] {"testZipFile.txt"}));
        }
        //[TestMethod]
        //[ExpectedException(typeof(FileNotFoundException), "Zipping a non existing file was ordered.")]
        //public void ZipFilesShouldThrowWhenFileIsNotFound()
        //{
        //    Methods.ZipFiles("exception", new []{"non existing file"});
        //}
    }
}
