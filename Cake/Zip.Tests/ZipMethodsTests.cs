using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zip.Tests
{

    [TestClass]
    public class ZipMethodsTests
    {
        private const string pathForTests = "../../Test Files/assert";


        [TestMethod]
        public void ZipFilesShouldReturnFailureIfFileDoesNotExist()
        {
            if(File.Exists(pathForTests+".zip"))
                File.Delete(pathForTests+".zip");
            Assert.AreEqual(false, Methods.ZipFiles(pathForTests, "Nonexisting or invalid project/solution file"));
        }

        [TestMethod]
        public void ZipFilesShoudReturnFailureIfFilePathIsEmpty()
        {
            if (File.Exists(pathForTests + ".zip"))
                File.Delete(pathForTests + ".zip");
            Assert.AreEqual(false, Methods.ZipFiles(pathForTests, ""));
        }

        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfParametersAreValid()
        {
            if (File.Exists(pathForTests + ".zip"))
                File.Delete(pathForTests + ".zip");
            Assert.AreEqual(true, Methods.ZipFiles(pathForTests, "../../Test Files/testZipFile.txt"));
        }

        [TestMethod]
        public void ZipFilesShouldReturnSuccessIfZipPathIsNotSpecified()
        {
            if (File.Exists(pathForTests + ".zip"))
                File.Delete(pathForTests + ".zip");
            Assert.AreEqual(true, Methods.ZipFiles(pathForTests, "../../Test Files/testZipFile.txt"));
        }

        [TestMethod]
        public void ZipFilesShouldReturnFailureWhenZipPathIsNotValid()
        {
            Assert.AreEqual(false, Methods.ZipFiles("invalid zip path:!@", "../../Test Files/testZipFile.txt"));
        }

        [TestMethod]
        public void ZipFilesShouldReturnSuccessWhenZippedAndUnzippedFilesAreSameAsNotZippedFiles()
        {
            const string unzippedPath = "../../Test Files/UnzippedFilesForTest";
            const string archivePath = "../../Test Files/sameContentTest";
            var filesToTest = new[] {"testZipFile.txt", "testZipFile2.txt"};
            if (Directory.Exists(unzippedPath))
                Directory.Delete(unzippedPath, true);
            if (File.Exists(archivePath+".zip"))
                File.Delete(archivePath+".zip");
            Methods.ZipFiles("../../Test Files/sameContentTest", "../../Test Files/testZipFile.txt", "../../Test Files/testZipFile2.txt");
            System.IO.Compression.ZipFile.ExtractToDirectory(Path.GetFullPath(archivePath) + ".zip", Path.GetFullPath(unzippedPath));
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
