using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Files.Tests
{
    [TestClass]
    public class FilesMethodsTests
    {
        private const string PathForTests = "../../Test Files/";
        [TestMethod]
        public void CopyFolderShouldReturnFalseIfSourcePathIsNotCorrect()
        {
            //Assert.AreEqual(false, Methods.CopyFolder("Incorrect path:!@$", PathForTests+"Folder To Copy"));
            Assert.IsFalse(Methods.CopyFolder("Incorrect path:!@$", PathForTests + "Folder To Copy"));
        }

        [TestMethod]
        public void CopyFolderShouldReturnFalseIfDestinationPathIsNotCorrect()
        {
            Assert.IsFalse(Methods.CopyFolder(PathForTests + "Folder To Copy", "Incorrect path:!@$@!"));
            //Assert.AreEqual(false, Methods.CopyFolder(PathForTests+"Folder To Copy", "Incorrect path:!@$@!"));
        }

        [TestMethod]
        public void CopyFolderWithoutSubdirectoriesShouldCopyOnlyFiles()
        {
            var filesToCopy = new DirectoryInfo(PathForTests).GetFiles();
            var dirToCopy = new DirectoryInfo(PathForTests).GetDirectories();
            foreach(var dir in Directory.GetDirectories(PathForTests+"Copied Content"))
                Directory.Delete(dir, true);
            //var res = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", false);
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", false));
            var filesCopied = new DirectoryInfo(PathForTests + "Copied Content").GetFiles();
            Assert.AreEqual(filesToCopy.Length, filesCopied.Length);
            var dirCopied = new DirectoryInfo(PathForTests + "Copied Content").GetDirectories();
            CollectionAssert.AreEqual(new DirectoryInfo[]{}, dirCopied);
            CollectionAssert.AreNotEqual(dirToCopy, dirCopied);
        }

        [TestMethod]
        public void CopyFolderOverwriteShouldOverwrite()
        {
            FileInfo f, f2;
            if (File.Exists(PathForTests + "Copied Content/FileToCopy.txt"))
            {
                if (File.Exists(PathForTests + "Copied Content/File2ToCopy.txt"))
                {
                    f = new FileInfo(PathForTests + "Copied Content/FileToCopy.txt");
                    f2 = new FileInfo(PathForTests + "Copied Content/File2ToCopy.txt");
                }
                else
                {
                    //var res = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content");
                    //Assert.IsTrue(res);
                    Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
                    f = new FileInfo(PathForTests + "Copied Content/FileToCopy.txt");
                    f2 = new FileInfo(PathForTests + "Copied Content/File2ToCopy.txt");
                }
            }
            else
            {
                //var res2 = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content");
                Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
                f = new FileInfo(PathForTests + "Copied Content/FileToCopy.txt");
                f2 = new FileInfo(PathForTests + "Copied Content/File2ToCopy.txt");
            }
            //var res3 = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", false, true);
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", false, true));
            var fa = new FileInfo(PathForTests + "Copied Content/FileToCopy.txt");
            var fa2 = new FileInfo(PathForTests + "Copied Content/File2ToCopy.txt");
            Assert.AreNotEqual(f, fa);
            Assert.AreNotEqual(f2, fa2);
        }

        [TestMethod]
        public void CopyFolderCleanDestinationDirectoryShouldDeleteAllPreviousContent()
        {
            Directory.Delete(PathForTests+"Copied Content", true);
            //var res = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", true, true);
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", true, true));
            var files = new DirectoryInfo(PathForTests + "Copied Content").GetFiles("*", SearchOption.AllDirectories);
            //res = Methods.CopyFolder(PathForTests, PathForTests+"Copied Content", false, false, true);
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", false, false, true));
            Assert.IsTrue(File.Exists(PathForTests + "Copied Content/FileToCopy.txt"));
            Assert.IsTrue(File.Exists(PathForTests + "Copied Content/File2ToCopy.txt"));
            //Assert.AreEqual(true, File.Exists(PathForTests + "Copied Content/FileToCopy.txt"));
            //Assert.AreEqual(true, File.Exists(PathForTests + "Copied Content/File2ToCopy.txt"));
            Assert.AreEqual(2, new DirectoryInfo(PathForTests+"Copied Content").GetFiles().Length);
            Assert.AreEqual(0, new DirectoryInfo(PathForTests+"Copied Content").GetDirectories().Length);
            Assert.AreNotEqual(files.Length, new DirectoryInfo(PathForTests + "Copied Content").GetFiles().Length);
        }

        [TestMethod]
        public void CopyFileShouldReturnFalseIfFilesDoesNotExist()
        {
            Assert.IsFalse(Methods.CopyFile(PathForTests + "not existing file", PathForTests + "Copied Content"));
            //Assert.AreEqual(false, Methods.CopyFile(PathForTests+"not existing file", PathForTests+"Copied Content"));
        }

        [TestMethod]
        public void CopyFileShouldReturnFalseIfFilePathIsNotCorrect()
        {
            Assert.IsFalse(Methods.CopyFile(PathForTests + ":@!@#", PathForTests + "Copied Content"));
            //Assert.AreEqual(false, Methods.CopyFile(PathForTests + ":@!@#", PathForTests + "Copied Content"));
        }

        [TestMethod]
        public void CopyFileShouldOverwritePreviousFile()
        {
            //var res = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content");
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
            var file = new FileInfo(PathForTests + "Copied Content/FileToCopy.txt");
            using (var wr = new StreamWriter(PathForTests + "FileToCopy.txt"))
            {
                wr.WriteLine("test");
            }
            Assert.IsTrue(Methods.CopyFile(PathForTests + "FileToCopy.txt", PathForTests + "Copied Content" + "/FileToCopy.txt"));
            Assert.AreNotEqual(file.Length, new FileInfo(PathForTests + "Copied Content/FileToCopy.txt").Length);
            //Assert.AreNotEqual(file, new FileInfo(PathForTests + "Copied Content/FileToCopy.txt"));
        }

        [TestMethod]
        public void CopyFileWithoutOverwriteShouldNotOverwritePreviousFile()
        {
            //var res = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content");
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
            var f = new FileInfo(PathForTests + "Copied Content/FileToCopy.txt");
            var fileList = new List<FileInfo> { f };

            var queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("Content")
                select file.FullName;


            //Assert.AreEqual(false, string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.IsFalse(string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Methods.ReplaceText(PathForTests + "Copied Content/FileToCopy.txt", "Content", "New content");


            queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("New content")
                select file.FullName;

            //Assert.AreEqual(false, string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
           Assert.IsFalse(string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            //Methods.CopyFile(PathForTests + "FileToCopy.txt",PathForTests + "Copied Content/", false);
           Assert.IsTrue(Methods.CopyFile(PathForTests + "FileToCopy.txt", PathForTests + "Copied Content/", false));
            queryMatchingFiles =
                from file in fileList
                where file.Extension == ".txt"
                let fileText = GetFileText(file.FullName)
                where fileText.Contains("New content")
                select file.FullName;

            //Assert.AreEqual(false, string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.IsTrue(string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.IsTrue(Methods.ReplaceText(PathForTests + "Copied Content/FileToCopy.txt", "New content", "Content"));

        }

        [TestMethod]
        public void DeleteFileShouldReturnFalseIfFileDoesNotExist()
        {
            //Assert.AreEqual(false,Methods.DeleteFile(PathForTests + "non existing file"));
            Assert.IsFalse(Methods.DeleteFile(PathForTests + "non existing file"));
        }

        [TestMethod]
        public void DeleteFileShouldDeleteFile()
        {
            Assert.IsTrue(Methods.CopyFile(PathForTests + "FileToCopy.txt", PathForTests + "Copied Content" + "/FileToCopy.txt"));
            Assert.IsTrue(Methods.DeleteFile(PathForTests + "Copied Content/FileToCopy.txt"));
            Assert.IsFalse(File.Exists(PathForTests + "Copied Content/FileToCopy.txt"));
            //Assert.AreEqual(false, File.Exists(PathForTests + "Copied Content/FileToCopy.txt"));
        }

        [TestMethod]
        public void GetFilesWithPatternShouldNotReturnFilePathsFromSubdirectories()
        {
            Assert.AreEqual(0, Methods.GetFilesWithPattern(PathForTests, "File3ToCopy.txt").Length);
        }

        [TestMethod]
        public void GetFilesWithPatternSubdirectoriesIncludedShouldReturnFilePathsFromSubdirectories()
        {
            if (Directory.Exists(PathForTests + "Copied Content"))
                Directory.Delete(PathForTests + "Copied Content", true);
            Assert.AreEqual(1, Methods.GetFilesWithPattern(PathForTests, "File3ToCopy.txt", true).Length);
        }

        [TestMethod]
        public void GetFilesWithPatternShouldReturnEmptyArrayIfParentDirectoryPathIsIncorrect()
        {
            Assert.AreEqual(0, Methods.GetFilesWithPattern("incorect path:!@@#Q#", "filePattern").Length);
        }

        [TestMethod]
        public void DeleteFilesWithPatternShouldReturnFalseIfParentDirectoryPathIsIncorrect()
        {
            Assert.IsFalse(Methods.DeleteFilesWithPattern("incorrect path:!@!#", "filePattern"));
            //Assert.AreEqual(false, Methods.DeleteFilesWithPattern("incorrect path:!@!#", "filePattern"));
        }

        [TestMethod]
        public void DeleteFilesWithPatternShouldDeleteOnlyFiles()
        {
            Assert.IsTrue(Methods.CleanDirectory(PathForTests + "Copied Content"));
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
            var dirs = new DirectoryInfo(PathForTests + "Copied Content").GetDirectories();
            var files = new DirectoryInfo(PathForTests + "Copied Content").GetFiles(); 
            Assert.IsTrue(Methods.DeleteFilesWithPattern(PathForTests + "Copied Content", "*.txt"));
            var dirsDel = new DirectoryInfo(PathForTests + "Copied Content").GetDirectories();
            var filesDel = new DirectoryInfo(PathForTests + "Copied Content").GetFiles();
            Assert.AreEqual(dirs.Length, dirsDel.Length);
            CollectionAssert.AreNotEqual(files, filesDel);
            Assert.AreNotEqual(files.Length, filesDel.Length);

        }

        [TestMethod]
        public void DeleteDirectoriesWithPatternShouldReturnFalseIfParentDirectoryPathIsIncorrect()
        {
            Assert.IsFalse(Methods.DeleteDirectoriesWithPattern("incorrect path:!@!#", "directoryPattern"));
            //Assert.AreEqual(false, Methods.DeleteDirectoriesWithPattern("incorrect path:!@!#", "directoryPattern"));
        }

        [TestMethod]
        public void DeleteDirectoriesWithPatternShouldReturnTrueIfSomeDirectoryIsNotEmpty()// CZYŻBY?
        {
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content", true, true));
            var dirInfos = new DirectoryInfo(PathForTests + "Copied Content").GetDirectories();
            Assert.AreNotEqual(dirInfos.Length, 0);
            var dirs = Directory.GetDirectories(PathForTests + "Copied Content");
            Assert.IsTrue(dirs.Any(d => !string.IsNullOrEmpty(d)));
            Assert.IsTrue(Methods.DeleteDirectoriesWithPattern(PathForTests + "Copied Content/", "Copied Content", true));
            //Assert.AreEqual(true, Methods.DeleteDirectoriesWithPattern(PathForTests + "Copied Content/", "Copied Content", true));
        }

        [TestMethod]
        public void DeleteDirectoryShouldReturnFalseIfDirectoryPathIsIncorrect()
        {
            Assert.IsFalse(Methods.DeleteDirectory("incorrect path :!@#"));
            //Assert.AreEqual(false, Methods.DeleteDirectory("incorrect path :!@#"));
        }

        [TestMethod]
        public void CleanDirectoryShouldNotDeleteParentFolder()
        {
            //var res = Methods.CopyFolder(PathForTests, PathForTests + "Copied Content");
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
            //var res2 = Methods.CleanDirectory(PathForTests + "Copied Content");
            Assert.IsTrue(Methods.CleanDirectory(PathForTests + "Copied Content"));
            Assert.IsTrue(Directory.Exists(PathForTests + "Copied Content"));
            //Assert.AreEqual(true, Directory.Exists(PathForTests + "Copied Content"));
        }

        [TestMethod]
        public void CleanDirectoryShouldDeleteContent()
        {
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
            var filesBefore = new DirectoryInfo(PathForTests + "Copied Content").GetFiles();
            var dirsBefore = new DirectoryInfo(PathForTests + "Copied Content").GetDirectories();
            Assert.IsTrue(Methods.CleanDirectory(PathForTests + "Copied Content"));
            var filesAfter = new DirectoryInfo(PathForTests + "Copied Content").GetFiles();
            var dirsAfter = new DirectoryInfo(PathForTests + "Copied Content").GetDirectories();
            CollectionAssert.AreNotEqual(filesBefore, filesAfter);
            CollectionAssert.AreNotEqual(dirsBefore, dirsAfter);
            Assert.IsTrue(Directory.Exists(PathForTests + "Copied Content"));
            //Assert.AreEqual(true, Directory.Exists(PathForTests + "Copied Content"));
            Assert.AreEqual(0, filesAfter.Length);
            Assert.AreEqual(0, dirsAfter.Length);
        }

        [TestMethod]
        public void ReplaceTextShouldReturnFalseIfFileDoesNotExist()
        {
            Assert.IsFalse(Methods.ReplaceText(PathForTests + "non existing file", "content", "new content"));
            //Assert.AreEqual(false, Methods.ReplaceText(PathForTests + "non existing file", "content", "new content"));
        }

        [TestMethod]
        public void ReplaceTextShouldOverwriteTextPattern()
        {
            var f = new FileInfo(PathForTests + "FileToCopy.txt");
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

            //Assert.AreEqual(false, string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.IsFalse(string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.IsTrue(string.IsNullOrEmpty(queryMatchingFiles2.FirstOrDefault()));
            //Assert.AreEqual(true, string.IsNullOrEmpty(queryMatchingFiles2.FirstOrDefault()));
            Assert.IsTrue(Methods.ReplaceText(PathForTests + "FileToCopy.txt", "Content", "New content"));
            

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

            //Assert.AreEqual(false, string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.IsFalse(string.IsNullOrEmpty(queryMatchingFiles.FirstOrDefault()));
            Assert.IsTrue(string.IsNullOrEmpty(queryMatchingFiles2.FirstOrDefault()));
            //Assert.AreEqual(true, string.IsNullOrEmpty(queryMatchingFiles2.FirstOrDefault()));


            Assert.IsTrue(Methods.ReplaceText(PathForTests + "FileToCopy.txt", "New content", "Content"));

        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnFileNameIfSomeFolderPathIsIncorrect()
        {
            var res = Methods.LookForFileInFolders("returned filename", "incorrect path:!@#P{", PathForTests);
            Assert.AreEqual("returned filename", res.FirstOrDefault());
            Assert.AreEqual(1, res.Length);
        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnFileNameIfFileWasNotFound()
        {
            var res = Methods.LookForFileInFolders("File3ToCopy.txt", PathForTests);
            Assert.AreEqual("File3ToCopy.txt", res.FirstOrDefault());
            Assert.AreEqual(1, res.Length);
        }

        [TestMethod]
        public void LookForFileInFoldersShouldReturnAllFilePaths()
        {
            Assert.IsTrue(Methods.CleanDirectory(PathForTests + "Copied Content"));
            Assert.IsTrue(Methods.CopyFolder(PathForTests, PathForTests + "Copied Content"));
            var res = Methods.LookForFileInFolders("FileToCopy.txt", PathForTests, PathForTests + "Copied Content");
            Assert.AreEqual(2, res.Length);
            foreach (var path in res)
            {
                Assert.IsTrue(path.Contains("FileToCopy.txt"));
                //Assert.AreEqual(true, path.Contains("FileToCopy.txt"));
            }
        }

        private static string GetFileText(string name)
        {
            var fileContents = string.Empty;

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
