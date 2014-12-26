using System.IO;
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
        /// <summary>
        /// Test method for invalid zip path in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnFailureWhenZipPathIsNotValid()
        {
            Assert.AreEqual(false, Methods.ZipFiles("exception", new[] { "testZipFile.txt" }, "invalid zip path"));
        }
        //TODO: przy ...ShouldReturnSuccess... zrobić dodatkowe testy sprawdzające identyczność plików przed i po zipowaniu
        /// <summary>
        /// Test method for successful comparison of zipped and unzipped files with not zipped files
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnSuccessWhenZippedAndUnzippedFilesAreSameAsNotZippedFiles()
        {
            const string unzippedPath = "UnzippedFliesForTest";
            var filesToTest = new[] {"testZipFile.txt", "testZipFile2.txt"};
            if (Directory.Exists(unzippedPath))
            {
                var dir = new DirectoryInfo(unzippedPath);
                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(unzippedPath);
            }
            Methods.ZipFiles("sameContentTest", new[] {"testZipFile.txt", "testZipFile2.txt"});
            System.IO.Compression.ZipFile.ExtractToDirectory("sameContentTest.zip", unzippedPath);
            foreach (var file in filesToTest)
            {

                var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                var fs2 = new FileStream(unzippedPath + "/" + file, FileMode.Open, FileAccess.Read);
                int notZippedFileByte, unzippedFileByte;
                Assert.AreEqual(fs.Length, fs2.Length);
                do
                {
                    notZippedFileByte = fs.ReadByte();
                    unzippedFileByte = fs2.ReadByte();
                    Assert.AreEqual(notZippedFileByte, unzippedFileByte);
                } while (notZippedFileByte != -1 && unzippedFileByte != -1);
            }
        }
    }
}
