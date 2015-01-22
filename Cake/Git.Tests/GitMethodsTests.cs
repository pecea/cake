using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Git.Tests
{
    [TestClass]
    public class GitMethodsTests
    {
        const string Path = "../../Test Files/UnitTestRepository";

        [TestInitialize]
        public void CreateRepository()
        {
            Methods.PathToRepository = Path;
            if (Directory.Exists(Path)) return;
            Directory.CreateDirectory(Path);
            Directory.SetCurrentDirectory(Path);
            Methods.Run("init");
            Methods.Run("add");
        }
        [TestMethod]
        public void TagShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.Tag("unitTest"));
            Assert.AreEqual(true, Methods.Tag("-d unitTest"));
        }

        [TestMethod]
        public void CleanShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.Clean());
        }

        [TestMethod]
        public void RunShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.Run("tag unitTest"));
            Assert.AreEqual(true, Methods.Run("tag -d unitTest"));
        }

        [TestMethod]
        public void CurrentBranchShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.CurrentBranch());
        }

        [TestMethod]
        public void CurrentShaShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.CurrentSha());
        }

        [TestCleanup]
        public void CleanUp()
        {
            try
            {
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
