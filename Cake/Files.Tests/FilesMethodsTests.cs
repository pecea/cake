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
            Directory.Delete(pathForTests+"Copied Content", true);
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content", true, true);
            var files = new DirectoryInfo(pathForTests + "Copied Content").GetFiles("*", SearchOption.AllDirectories);
            Methods.CopyFolder(pathForTests, pathForTests+"Copied Content", false, false, true);
            Assert.AreEqual(2, new DirectoryInfo(pathForTests+"Copied Content").GetFiles().Count());
            Assert.AreEqual(0, new DirectoryInfo(pathForTests+"Copied Content").GetDirectories().Count());
            Assert.AreNotEqual(files.Count(), new DirectoryInfo(pathForTests + "Copied Content").GetFiles().Count());
        }

        [TestMethod]
        public void CopyFileShouldReturnFalseIfFilesDoesNotExist()
        {
            Assert.AreEqual(false, Methods.CopyFile(pathForTests+"not existing file", pathForTests+"Copied Content"));
        }

        [TestMethod]
        public void CopyFileShouldReturnFalseIfFilePathIsNotCorrect()
        {
            Assert.AreEqual(false, Methods.CopyFile(pathForTests + ":@!@#", pathForTests + "Copied Content"));
        }

        [TestMethod]
        public void CopyFileShouldOverwritePreviousFile()
        {
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            var file = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
            Methods.CopyFile(pathForTests + "FileToCopy.txt", pathForTests + "Copied Content");
            Assert.AreNotEqual(file, new FileInfo(pathForTests + "Copied Content/FileToCopy.txt"));
        }

        //[TestMethod]
        //public void CopyFileWithoutOverwriteShouldNotOverwritePreviousFile()
        //{
        //    Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
        //    var file = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
        //    Methods.CopyFile(pathForTests + "FileToCopy.txt", pathForTests + "Copied Content", false);
        //    Assert.AreEqual(file, new FileInfo(pathForTests + "Copied Content/FileToCopy.txt"));
        //}

        [TestMethod]
        public void DeleteFileShouldReturnFalseIfFileDoesNotExist()
        {
            Assert.AreEqual(false,Methods.DeleteFile(pathForTests + "non existing file"));
        }

        [TestMethod]
        public void DeleteFileShouldDeleteFile()
        {
            Methods.CopyFile(pathForTests + "FileToCopy.txt", pathForTests + "Copied Content");
            Methods.DeleteFile(pathForTests + "Copied Content/FileToCopy.txt");
            Assert.AreEqual(false, File.Exists(pathForTests + "Copied Content/FileToCopy.txt"));
        }

        [TestMethod]
        public void GetFilesWithPatternShouldNotReturnFilePathsFromSubdirectories()
        {
            Assert.AreEqual(0, Methods.GetFilesWithPattern(pathForTests, "File3ToCopy.txt").Count());
        }

        //[TestMethod]
        //public void GetFilesWithPatternSubdirectoriesIncludedShouldReturnFilePathsFromSubdirectories()
        //{
        //    Assert.AreEqual(1, Methods.GetFilesWithPattern(pathForTests, "File3ToCopy.txt", true).Count());
        //}

        [TestMethod]
        public void GetFilesWithPatternShouldReturnEmptyArrayIfParentDirectoryPathIsIncorrect()
        {
            Assert.AreEqual(0, Methods.GetFilesWithPattern("incorect path:!@@#Q#", "filePattern").Count());
        }

        [TestMethod]
        public void DeleteFilesWithPatternShouldReturnFalseIfParentDirectoryPathIsIncorrect()
        {
            Assert.AreEqual(false, Methods.DeleteFilesWithPattern("incorrect path:!@!#", "filePattern"));
        }

        [TestMethod]
        public void DeleteFilesWithPatternShouldDeleteOnlyFiles()
        {
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            var dirs = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            var files = new DirectoryInfo(pathForTests + "Copied Content").GetFiles(); 
            Methods.DeleteFilesWithPattern(pathForTests + "Copied Content", "Copy");
            var dirsDel = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            var filesDel = new DirectoryInfo(pathForTests + "Copied Content").GetFiles();
            CollectionAssert.AreEqual(dirs, dirsDel);
            CollectionAssert.AreNotEqual(files, filesDel);

        }

        [TestMethod]
        public void DeleteDirectoriesWithPatternShouldReturnFalseIfParentDirectoryPathIsIncorrect()
        {
            Assert.AreEqual(false, Methods.DeleteDirectoriesWithPattern("incorrect path:!@!#", "directoryPattern"));
        }

        [TestMethod]
        public void DeleteDirectoriesWithPatternShouldReturnTrueIfSomeDirectoryIsNotEmpty()// CZYŻBY?
        {
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content", true, true);
            var dirInfos = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            var dirs = Directory.GetDirectories(pathForTests + "Copied Content");
            Assert.AreEqual(true, Methods.DeleteDirectoriesWithPattern(pathForTests + "Copied Content/", "Copied Content", true));
        }

        //[TestMethod]
        //public void DeleteDirectoryShouldDeleteDirectory()
        //{
            
        //}

        [TestMethod]
        public void DeleteDirectoryShouldReturnFalseIfDirectoryPathIsIncorrect()
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
