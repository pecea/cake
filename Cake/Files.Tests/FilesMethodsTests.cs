using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Files.Tests
{
    [TestClass]
    public class FilesMethodsTests
    {
        [TestMethod]
        public void CopyFolderShouldReturnFalseIfSourcePathIsNotCorrect()
        {
            
        }

        [TestMethod]
        public void CopyFolderShouldReturnFalseIfDestinationPathIsNotCorrect()
        {
            
        }

        [TestMethod]
        public void CopyFolderWithoutSubdirectoriesShouldCopyOnlyFiles()
        {
            
        }

        [TestMethod]
        public void CopyFolderOverwriteShouldOverwrite()
        {
            
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
