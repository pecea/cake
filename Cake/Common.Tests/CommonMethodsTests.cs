namespace Common.Tests
{
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommonMethodsTests
    {
        [TestMethod]
        public void PathParserGetFilePathsShouldReturnFilesOnly()
        {
            var files = @"..\..\Test Files\**\*".GetFilePaths();

            foreach (var file in files)
            {
                Assert.AreEqual(false, Directory.Exists(file));
                Assert.AreEqual(true, File.Exists(file));
            }
        }


    }
}
