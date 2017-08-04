namespace Common.Tests
{
    using System.IO;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    /// <summary>
    /// Test class for Common methods
    /// </summary>
    [TestClass]
    public class CommonMethodsTests
    {
        /// <summary>
        /// Test method for parsing file paths
        /// </summary>
        [TestMethod]
        [TestCategory("CommonMethods")]
        public void PathParserGetFilePathsShouldReturnFilesOnly()
        {
            var files = @"..\..\Test Files\**\*".GetFilePaths();

            foreach (var file in files)
            {
                Assert.IsFalse(Directory.Exists(file));
                Assert.IsTrue(File.Exists(file));
            }
        }
        /// <summary>
        /// Test method for parsing directory paths
        /// </summary>
        [TestMethod]
        [TestCategory("CommonMethods")]
        public void PathParserGetDirectoriesPathsShouldReturnDirectoriesOnly()
        {
            var directories = @"..\..\Test Files\**".GetDirectoriesPaths();

            foreach (var directory in directories)
            {
                Assert.IsFalse(File.Exists(directory));
                Assert.IsTrue(Directory.Exists(directory));
            }
        }
        /// <summary>
        /// Test method for non-existing directory path
        /// </summary>
        [TestMethod]
        [TestCategory("CommonMethods")]
        public void PathParserGetDirectoriesPathsShouldReturnZeroItemsIfNonExistingPathIsSpecified()
        {
            var result = @"..\..\Test Files\**\Non existing folder\*".GetDirectoriesPaths();
            Assert.AreEqual(0, result.Count());
        }        
        /// <summary>
        /// Test method for non-existing file path
        /// </summary>
        [TestMethod]
        [TestCategory("CommonMethods")]
        public void PathParserGetFilesPathsShouldReturnZeroItemsIfNonExistingPathIsSpecified()
        {
            var result = @"..\..\Test Files\**\Non existing folder\*".GetFilePaths();
            Assert.AreEqual(0, result.Count());
        }
        /// <summary>
        /// Test method for invalid directory path
        /// </summary>
        [TestMethod]
        [TestCategory("CommonMethods")]
        public void PathParserGetDirectoriesPathsShouldReturnZeroItemsIfInvalidPathIsSpecified()
        {
            var result = @":\?.:\**".GetDirectoriesPaths();
            Assert.AreEqual(0, result.Count());
        }
        /// <summary>
        /// Test method for invalid file path
        /// </summary>
        [TestMethod]
        [TestCategory("CommonMethods")]
        public void PathParserGetFilesPathsShouldReturnZeroItemsIfInvalidPathIsSpecified()
        {
            var result = @":\?.:\*.*".GetFilePaths();
            Assert.AreEqual(0, result.Count());
        }
    }
}
