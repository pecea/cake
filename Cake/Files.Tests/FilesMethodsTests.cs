using System;
using System.Collections.Generic;
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
            FileInfo f, f2;
            if (File.Exists(pathForTests + "Copied Content/FileToCopy.txt"))
            {
                if (File.Exists(pathForTests + "Copied Content/File2ToCopy.txt"))
                {
                    f = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
                    f2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt");
                }
                else
                {
                    Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
                    f = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
                    f2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt");
                }
            }
            else
            {
                Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
                f = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
                f2 = new FileInfo(pathForTests + "Copied Content/File2ToCopy.txt");
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
            Assert.AreEqual(true, File.Exists(pathForTests + "Copied Content/FileToCopy.txt"));
            Assert.AreEqual(true, File.Exists(pathForTests + "Copied Content/File2ToCopy.txt"));
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

        [TestMethod]
        public void CopyFileWithoutOverwriteShouldNotOverwritePreviousFile()
        {
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            var f = new FileInfo(pathForTests + "Copied Content/FileToCopy.txt");
            var fileList = new List<FileInfo> { f };

            var queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("Content")
                select file.FullName;


            Assert.AreEqual(false, String.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            
            Methods.ReplaceText(pathForTests + "Copied Content/FileToCopy.txt", "Content", "New content");


            queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("New content")
                select file.FullName;

            Assert.AreEqual(false, String.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
           
            Methods.CopyFile(pathForTests + "FileToCopy.txt",pathForTests + "Copied Content/", false);
           
            queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("New content")
                select file.FullName;

            Assert.AreEqual(false, String.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            
            Methods.ReplaceText(pathForTests + "Copied Content/FileToCopy.txt", "New content", "Content");

        }

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

        [TestMethod]
        public void GetFilesWithPatternSubdirectoriesIncludedShouldReturnFilePathsFromSubdirectories()
        {
            if (Directory.Exists(pathForTests + "Copied Content"))
                Directory.Delete(pathForTests + "Copied Content", true);
            Assert.AreEqual(1, Methods.GetFilesWithPattern(pathForTests, "File3ToCopy.txt", true).Count());
        }

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
            Methods.CleanDirectory(pathForTests + "Copied Content");
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            var dirs = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            var files = new DirectoryInfo(pathForTests + "Copied Content").GetFiles(); 
            Methods.DeleteFilesWithPattern(pathForTests + "Copied Content", "*.txt");
            var dirsDel = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            var filesDel = new DirectoryInfo(pathForTests + "Copied Content").GetFiles();
            Assert.AreEqual(dirs.Count(), dirsDel.Count());
            CollectionAssert.AreNotEqual(files, filesDel);
            Assert.AreNotEqual(files.Count(), filesDel.Count());

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

        [TestMethod]
        public void DeleteDirectoryShouldReturnFalseIfDirectoryPathIsIncorrect()
        {
            Assert.AreEqual(false, Methods.DeleteDirectory("incorrect path :!@#"));
        }

        [TestMethod]
        public void CleanDirectoryShouldNotDeleteParentFolder()
        {
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            Methods.CleanDirectory(pathForTests + "Copied Content");
            Assert.AreEqual(true, Directory.Exists(pathForTests + "Copied Content"));
        }

        [TestMethod]
        public void CleanDirectoryShouldDeleteContent()
        {
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            var filesBefore = new DirectoryInfo(pathForTests + "Copied Content").GetFiles();
            var dirsBefore = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            Methods.CleanDirectory(pathForTests + "Copied Content");
            var filesAfter = new DirectoryInfo(pathForTests + "Copied Content").GetFiles();
            var dirsAfter = new DirectoryInfo(pathForTests + "Copied Content").GetDirectories();
            CollectionAssert.AreNotEqual(filesBefore, filesAfter);
            CollectionAssert.AreNotEqual(dirsBefore, dirsAfter);
            Assert.AreEqual(true, Directory.Exists(pathForTests + "Copied Content"));
            Assert.AreEqual(0, filesAfter.Count());
            Assert.AreEqual(0, dirsAfter.Count());
        }

        [TestMethod]
        public void ReplaceTextShouldReturnFalseIfFileDoesNotExist()
        {
            Assert.AreEqual(false, Methods.ReplaceText(pathForTests + "non existing file", "content", "new content"));
        }

        [TestMethod]
        public void ReplaceTextShouldOverwriteTextPattern()
        {
            var f = new FileInfo(pathForTests + "FileToCopy.txt");
            var fileList = new List<FileInfo> {f};

            var queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("Content")
                select file.FullName;
            

            var queryMatchingFiles2 =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("New content")
                select file.FullName;

            Assert.AreEqual(false, String.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.AreEqual(true, String.IsNullOrEmpty(queryMatchingFiles2.FirstOrDefault()));
            Methods.ReplaceText(pathForTests + "FileToCopy.txt", "Content", "New content");
            

            queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("New content")
                select file.FullName;
            queryMatchingFiles2 =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("Content")
                select file.FullName;

            Assert.AreEqual(false, String.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.AreEqual(true, String.IsNullOrEmpty(queryMatchingFiles2.FirstOrDefault()));


            Methods.ReplaceText(pathForTests + "FileToCopy.txt", "New content", "Content");

        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnFileNameIfSomeFolderPathIsIncorrect()
        {
            var res = Methods.LookForFileInFolders("returned filename", "incorrect path:!@#P{", pathForTests);
            Assert.AreEqual("returned filename", res.FirstOrDefault());
            Assert.AreEqual(1, res.Count());
        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnFileNameIfFileWasNotFound()
        {
            var res = Methods.LookForFileInFolders("File3ToCopy.txt", pathForTests);
            Assert.AreEqual("File3ToCopy.txt", res.FirstOrDefault());
            Assert.AreEqual(1, res.Count());
        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnAllFilePaths()
        {
            Methods.CleanDirectory(pathForTests + "Copied Content");
            Methods.CopyFolder(pathForTests, pathForTests + "Copied Content");
            var res = Methods.LookForFileInFolders("FileToCopy.txt", pathForTests, pathForTests + "Copied Content");
            Assert.AreEqual(2, res.Count());
            foreach (var path in res)
            {
                Assert.AreEqual(true, path.Contains("FileToCopy.txt"));
            }
        }

        private static string GetFileText(string name)
        {
            var fileContents = String.Empty;

            // If the file has been deleted since we took  
            // the snapshot, ignore it and return the empty string. 
            if (File.Exists(name))
            {
                fileContents = File.ReadAllText(name);
            }
            return fileContents;
        }
    }
}
