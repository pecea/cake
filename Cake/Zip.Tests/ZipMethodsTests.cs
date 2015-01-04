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
        private const string pathForTests = "../../Test Files/assert";

        /// <summary>
        /// Test method for non-existing file in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnFailureIfFileDoesNotExist()
        {
            if(File.Exists(pathForTests+".zip"))
                File.Delete(pathForTests+".zip");
            Assert.AreEqual(false, Methods.ZipFiles(pathForTests, "Nonexisting or invalid project/solution file"));
        }
        /// <summary>
        /// Test method for empty file path in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShoudReturnFailureIfFilePathIsEmpty()
        {
            if (File.Exists(pathForTests + ".zip"))
                File.Delete(pathForTests + ".zip");
            Assert.AreEqual(false, Methods.ZipFiles(pathForTests, ""));
        }
        /// <summary>
        /// Test method for successful zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfParametersAreValid()
        {
            if (File.Exists(pathForTests + ".zip"))
                File.Delete(pathForTests + ".zip");
            Assert.AreEqual(true, Methods.ZipFiles(pathForTests, "../../Test Files/testZipFile.txt"));
        }
        /// <summary>
        /// Test method for unspecified zip path in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfZipPathIsNotSpecified()
        {
            if (File.Exists(pathForTests + ".zip"))
                File.Delete(pathForTests + ".zip");
            Assert.AreEqual(true, Methods.ZipFiles(pathForTests, "../../Test Files/testZipFile.txt"));
        }
        /// <summary>
        /// Test method for invalid zip path in zip files method
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnFailureWhenZipPathIsNotValid()
        {
            Assert.AreEqual(false, Methods.ZipFiles("invalid zip path:!@", "../../Test Files/testZipFile.txt"));
        }
        //TODO: przy ...ShouldReturnSuccess... zrobić dodatkowe testy sprawdzające identyczność plików przed i po zipowaniu
        /// <summary>
        /// Test method for successful comparison of zipped and unzipped files with not zipped files
        /// </summary>
        [TestMethod]
        public void ZipFilesShouldReturnSuccessWhenZippedAndUnzippedFilesAreSameAsNotZippedFiles()
        {
            const string unzippedPath = "../../Test Files/UnzippedFliesForTest";
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
            if (File.Exists("../../Test Files/sameContentTest.zip"))
                File.Delete("../../Test Files/sameContentTest.zip");
            Methods.ZipFiles("../../Test Files/sameContentTest", "../../Test Files/testZipFile.txt", "../../Test Files/testZipFile2.txt");
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
