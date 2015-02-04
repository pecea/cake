using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Git.Tests
{
    [TestClass]
    public class GitMethodsTests
    {
        const string Path = @"..\..\Test Files\UnitTestRepository\";

        [TestInitialize]
        public void CreateRepository()
        {
            //Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());
            //Methods.PathToRepository = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            Directory.SetCurrentDirectory(Path);
            Methods.Run("init");
            Methods.Run("add");
            //Methods.Run("commit");
        }

        [TestMethod]
        public void TagShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(Path);
            Assert.AreEqual(true, Methods.Tag("unitTestTagut"));
            Assert.AreEqual(true, Methods.Tag("-d unitTestTagut"));
        }

        [TestMethod]
        public void CleanShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(Path);
            Assert.AreEqual(true, Methods.Clean());
        }

        [TestMethod]
        public void RunShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(Path);
            Assert.AreEqual(true, Methods.Run("tag unitTestTagut"));
            Assert.AreEqual(true, Methods.Run("tag -d unitTestTagut"));
        }

        [TestMethod]
        public void CurrentBranchShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(Path);
            Assert.AreEqual(true, Methods.CurrentBranch());
        }

        [TestMethod]
        public void CurrentShaShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(Path);
            Assert.AreEqual(true, Methods.CurrentSha());
        }

        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                Directory.SetCurrentDirectory(Path);
                Methods.Clean(true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }
        }
    }
}
