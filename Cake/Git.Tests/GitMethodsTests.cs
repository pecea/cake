using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Git.Tests
{
    [TestClass]
    public class GitMethodsTests
    {
        const string path = "../../Test Files/UnitTestRepository";

        [TestInitialize]
        public void CreateRepository()
        {
            //const string path = "../../Test Files/UnitTestRepository";
            Methods.PathToRepository = path;
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
            Directory.SetCurrentDirectory(path);
            Methods.Run("init");
            Methods.Run("add");
        }
        [TestMethod]
        public void TagShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(path);
            //Assert.AreEqual(0,0);
            Methods.PathToRepository = path;
            Assert.AreEqual(true, Methods.Tag("unitTest"));
            Assert.AreEqual(true, Methods.Tag("-d unitTest"));
        }

        //[TestMethod]
        //public void ResetAllModificationsShouldReturnSucces()
        //{
        //    Methods.PathToRepository = "../../Test Files/testRepository";
        //    Assert.AreEqual(true, Methods.ResetAllModifications());
        //}

        //[TestMethod]
        //public void PushShouldReturnSuccess()
        //{
        //    Assert.AreEqual(true, Methods.Push(@"C:\Users\ernes_000\Source\Repos\Praca Inżynierska\Cake\Git.Tests\bin\Debug\testForCake"));
        //}

        [TestMethod]
        public void CleanShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(path);
            Methods.PathToRepository = path;
            Assert.AreEqual(true, Methods.Clean());
        }

        [TestMethod]
        public void RunShouldReturnSuccess()
        {
            Directory.SetCurrentDirectory(path);
            Methods.PathToRepository = path;
            Assert.AreEqual(true, Methods.Run("tag unitTest"));
            Assert.AreEqual(true, Methods.Run("tag -d unitTest"));
        }

        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                Directory.SetCurrentDirectory(path);
                Methods.PathToRepository = path;
                Methods.Clean(true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }
        }
        //[TestCleanup]
        //public void AfterTests()
        //{
        //    Methods.Clean(true);
        //    Directory.Delete(path, true);
        //}
        //TODO: tworzyć repo przy testach (może test init)
    }
}
