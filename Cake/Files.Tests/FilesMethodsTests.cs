using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Files.Tests
{
    [TestClass]
    public class FilesMethodsTests
    {
        private const string pathForTests = "../../Test Files/";
        [TestMethod]
        public void CopyFolderShouldReturnFalseIfSourcePathIsNotCorrect()
        {
            Assert.AreEqual(false, Methods.CopyFolder("Incorrect path:!@$", pathForTests+"Folder To Copy"));
        }

        [TestMethod]
        public void CopyFolderShouldReturnFalseIfDestinationPathIsNotCorrect()
        {
            Assert.AreEqual(false, Methods.CopyFolder(pathForTests+"Folder To Copy", "Incorrect path:!@$@!"));
        }

        [TestMethod]
        public void CopyFolderWithoutSubdirectoriesShouldCopyOnlyFiles()
        {
            var filesToCopy = new DirectoryInfo(pathForTests).GetFiles();
            var dirToCopy = new DirectoryInfo(pathForTests).GetDirectories();
            foreach(var dir in Directory.GetDirectories(pathForTests+"Copied Content"))
                Directory.Delete(dir, true);
            //foreach (var file in Directory.GetFiles(pathForTests + "Copied Content"))
            //    File.Delete(file);
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content", false);
            var filesCopied = new DirectoryInfo(pathForTests + "Copied Content").GetFiles();
            Assert.AreEqual(filesToCopy.Count(), filesCopied.Count());
            var dirCopied = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            CollectionAssert.AreEqual(new DirectoryInfo[]{}, dirCopied);
            CollectionAssert.AreNotEqual(dirToCopy, dirCopied);
        }

        [TestMethod]
        public void CopyFolderOverwriteShouldOverwrite()
        {
            //DateTime ctBefore, ctBefore2;
            FileInfo f, f2;
            if (File.Exists(pathForTests + "Copied Content/FileToCopy.txt"))
            {
                if (File.Exists(pathForTests + "Copied Content/File2ToCopy.txt"))
                {
                    f = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
                    f2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt");
                    //ctBefore = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt").LastAccessTime;
                    //ctBefore2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt").LastAccessTime;
                }
                else
                {
                    Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
                    f = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
                    f2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt");
                    //ctBefore = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt").LastAccessTime;
                    //ctBefore2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt").LastAccessTime;

                }
            }
            else
            {
                Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
                f = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
                f2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt");
                //ctBefore = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt").LastAccessTime;
                //ctBefore2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt").LastAccessTime;

            }
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content", false, true);
            var fa = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
            var fa2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt");
            Assert.AreNotEqual(f, fa);
            Assert.AreNotEqual(f2, fa2);
        }

        [TestMethod]
        public void CopyFolderCleanDestinationDirectoryShouldDeleteAllPreviousContent()
        {
            
        }

        [TestMethod]
        public void CopyFileShouldReturnFalseIfFilesDoesNotExist()
        {
            
        }

        [TestMethod]
        public void CopyFileShouldOverwritePreviousFile()
        {

        }

        [TestMethod]
        public void CopyFileWithoutOverwriteShouldNotOverwritePreviousFile()
        {
            
        }

        [TestMethod]
        public void DeleteFileShouldReturnFalseIfFileDoesNotExist()
        {
            
        }

        [TestMethod]
        public void DeleteFileShouldDeleteFile()
        {
            
        }

        [TestMethod]
        public void GetFilesWithPatternShouldNotReturnFilePathsFromSubdirectories()
        {
            
        }

        [TestMethod]
        public void GetFilesWithPatternSubdirectoriesIncludedShouldReturnFilePathsFromSubdirectories()
        {
            
        }

        [TestMethod]
        public void GetFilesWithPatternShouldReturnFalseIfParentDirectoryPathIsIncorrect()
        {
            
        }

        [TestMethod]
        public void DeleteFilesWithPatternShouldReturnFalseIfParentDirectoryPathIsIncorrect()
        {

        }

        [TestMethod]
        public void DeleteFilesWithPatternShouldDeleteOnlyFiles()
        {
            
        }

        [TestMethod]
        public void DeleteDirectoriesWithPatternShouldReturnFalseIfParentDirectoryPathIsIncorrect()
        {

        }

        [TestMethod]
        public void DeleteDirectoriesWithPatternShouldReturnFalseIfSomeDirectoryIsNotEmpty()// CZYŻBY?
        {
            
        }

        [TestMethod]
        public void DeleteDirectoryShouldDeleteDirectory()
        {
            
        }

        [TestMethod]
        public void DeleteDirectoryShouldReturnFalseIfsDirectoryPathIsIncorrect()
        {

        }

        [TestMethod]
        public void DeleteDirectoryShouldReturnFalseIfFolderIsNotEmpty()// CZYŻBY?
        {

        }

        [TestMethod]
        public void CleanDirectoryShouldNotDeleteParentFolder()
        {
            
        }

        [TestMethod]
        public void CleanDirectoryShouldReturnFalseIfFolderContainsFiles()
        {

        }

        [TestMethod]
        public void CleanDirectoryShouldDeleteContent()
        {

        }

        [TestMethod]
        public void ReplaceTextShouldReturnFalseIfFileDoesNotExist()
        {
            
        }

        [TestMethod]
        public void ReplaceTextShouldOverwriteTextPattern()
        {

        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnFileNameIfSomeFolderPathIsIncorrect()
        {
            
        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnFileNameIfFileWasNotFound()
        {

        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnAllFilePaths()
        {

        }
    }
}
