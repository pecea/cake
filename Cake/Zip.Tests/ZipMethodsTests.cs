using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zip.Tests
{
    /// <summary>
    /// Test class for Zip methods
    /// </summary>
    [TestClass]
    public class ZipMethodsTests
    {
        private const string PathForTests = "../../Test Files/assert";

        /// <summary>
        /// Initialize method for deleting previously zipped files
        /// </summary>
        [TestInitialize]
        public void DeleteZippedFiles()
        {
            if (File.Exists(PathForTests + ".zip"))
                File.Delete(PathForTests + ".zip");
        }
        /// <summary>
        /// Test method for zipping non-existing files
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void ZipFilesShouldReturnFailureIfFileDoesNotExist()
        {
            //if(File.Exists(PathForTests+".zip"))
            //    File.Delete(PathForTests+".zip");
            Assert.IsFalse(Methods.ZipFiles(PathForTests, "Nonexisting or invalid project/solution file"));
        }
        /// <summary>
        /// Test method for files with empty path
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void ZipFilesShouldReturnFailureIfFilePathIsEmpty()
        {
            //if (File.Exists(PathForTests + ".zip"))
            //    File.Delete(PathForTests + ".zip");
            Assert.IsFalse(Methods.ZipFiles(PathForTests, ""));
        }
        /// <summary>
        /// Test method for zipping files
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void ZipFilesShouldReturnSuccessIfParametersAreValid()
        {
            //if (File.Exists(PathForTests + ".zip"))
            //    File.Delete(PathForTests + ".zip");
            Assert.IsTrue(Methods.ZipFiles(PathForTests, "../../Test Files/testZipFile.txt"));
        }
        /// <summary>
        /// Test method for zipping files without the path
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void ZipFilesShouldReturnSuccessIfZipPathIsNotSpecified()
        {
            //if (File.Exists(PathForTests + ".zip"))
            //    File.Delete(PathForTests + ".zip");
           // Assert.IsTrue(Methods.ZipFiles(PathForTests, "../../Test Files/testZipFile.txt"));
            Assert.IsTrue(Methods.ZipFiles(null, "../../Test Files/testZipFile.txt"));
        }
        /// <summary>
        /// Test method for zipping files with invalid path
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void ZipFilesShouldReturnFailureWhenZipPathIsNotValid()
        {
            Assert.IsFalse(Methods.ZipFiles("invalid zip path:!@", "../../Test Files/testZipFile.txt"));
        }
        /// <summary>
        /// Test method for zipping, unzipping files and comparing them with not-zipped files
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void ZipFilesShouldReturnSuccessWhenZippedAndUnzippedFilesAreSameAsNotZippedFiles()
        {
            const string unzippedPath = "../../Test Files/UnzippedFilesForTest";
            const string archivePath = "../../Test Files/sameContentTest";
            var filesToTest = new[] {"testZipFile.txt", "testZipFile2.txt"};
            if (Directory.Exists(unzippedPath))
                Directory.Delete(unzippedPath, true);
            if (File.Exists(archivePath+".zip"))
                File.Delete(archivePath+".zip");
            Assert.IsTrue(Methods.ZipFiles("../../Test Files/sameContentTest", "../../Test Files/testZipFile.txt", "../../Test Files/testZipFile2.txt"));
            System.IO.Compression.ZipFile.ExtractToDirectory(Path.GetFullPath(archivePath) + ".zip", Path.GetFullPath(unzippedPath));
            foreach (var file in filesToTest)
            {

                var fs = new FileStream("../../Test Files/"+file, FileMode.Open, FileAccess.Read);
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
