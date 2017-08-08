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
            Assert.IsTrue(Methods.ExtractFiles(Path.GetFullPath($"{archivePath}.zip"), Path.GetFullPath(unzippedPath)));
            //System.IO.Compression.ZipFile.ExtractToDirectory(Path.GetFullPath(archivePath) + ".zip", Path.GetFullPath(unzippedPath));
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
        /// <summary>
        /// Test method for deleting entries in an archive.
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void DeleteEntriesFromArchiveShouldDeleteEntries()
        {
            const string unzippedPath = "../../Test Files/DeleteTest/";
            if (Directory.Exists(unzippedPath))
                Directory.Delete(unzippedPath, true);

            Assert.IsTrue(Methods.ZipFiles(PathForTests, "../../Test Files/testZipFile.txt", "../../Test Files/testZipFile2.txt"));
            Assert.IsTrue(Methods.DeleteEntries($"{PathForTests}.zip", "testZipFile.txt"));
            Assert.IsTrue(Methods.ExtractFiles($"{PathForTests}.zip", unzippedPath));
            var files = Files.Methods.GetFilesWithPattern(unzippedPath, "*", true);
            Assert.AreEqual(1, files?.Length);
        }
        /// <summary>
        /// Test method for updating entries in an archive.
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void UpdateEntriesInArchiveShouldUpdateEntries()
        {
            const string testDirectory = "../../Test Files/UpdateTest/";
            const string unzippedPath = testDirectory + "Unzipped/";

            if (!Directory.Exists(testDirectory))
                Directory.CreateDirectory(testDirectory);
            if (Directory.Exists(unzippedPath))
                Directory.Delete(unzippedPath, true);
            Assert.IsTrue(Files.Methods.CleanDirectory(testDirectory));
            Assert.IsTrue(Files.Methods.CopyFile("../../Test Files/testZipFile.txt", testDirectory+"testZipFile.txt"));
            Assert.IsTrue(Methods.ZipFiles(PathForTests, testDirectory));
            Assert.IsTrue(Methods.ExtractFiles(PathForTests + ".zip", unzippedPath, null, true));
            var filesBeforeUpdate = Files.Methods.GetFilesWithPattern(unzippedPath, "*", true);
            Directory.Delete(unzippedPath, true);

            Assert.IsTrue(Files.Methods.CopyFile("../../Test Files/testZipFile2.txt", testDirectory+"testZipFile2.txt"));

            Assert.IsTrue(Methods.UpdateEntries(PathForTests+".zip", testDirectory));
            Assert.IsTrue(Methods.ExtractFiles(PathForTests + ".zip", unzippedPath, null, true));
            var filesAfterUpdate = Files.Methods.GetFilesWithPattern(unzippedPath, "*", true);
            CollectionAssert.DoesNotContain(filesBeforeUpdate, unzippedPath+"UpdateTest/testZipFile2.txt");
            Assert.AreNotEqual(filesBeforeUpdate?.Length, filesAfterUpdate?.Length);
            CollectionAssert.Contains(filesAfterUpdate, unzippedPath + "UpdateTest/testZipFile2.txt");
        }
        /// <summary>
        /// Test method for renaming an entry in archive.
        /// </summary>
        [TestMethod]
        [TestCategory("ZipMethods")]
        public void RenameEntryInArchiveShouldRenameEntry()
        {
            const string unzippedPath = "../../Test Files/RenameTest/";
            if(File.Exists(unzippedPath + "newName.txt"))
                File.Delete(unzippedPath + "newName.txt");
            Assert.IsTrue(Methods.ZipFiles(PathForTests, "../../Test Files/testZipFile.txt"));
            Assert.IsTrue(Methods.RenameEntry(PathForTests + ".zip", "testZipFile.txt", "newName.txt"));
            Assert.IsTrue(Methods.ExtractFiles(PathForTests + ".zip", unzippedPath));
            var filesAfterRename = Files.Methods.GetFilesWithPattern(unzippedPath, "*");
            CollectionAssert.Contains(filesAfterRename, unzippedPath + "newName.txt");

        }
    }
}
