using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Git.Tests
{
    [TestClass]
    public class GitMethodsTests
    {
        [TestMethod]
        public void TagShouldReturnSuccess()
        {
            Methods.PathToRepository = "../../Test Files/testRepository";
            Assert.AreEqual(true, Methods.Tag("unitTestTag"));
            Assert.AreEqual(true, Methods.Tag("-d unitTestTag"));
        }

        [TestMethod]
        public void CleanShouldReturnSuccess()
        {
            Methods.PathToRepository = "../../Test Files/testRepository";
            Assert.AreEqual(true, Methods.Clean());
        }

        [TestMethod]
        public void RunShouldReturnSuccess()
        {
            Methods.PathToRepository = "../../Test Files/testRepository";
            Assert.AreEqual(true, Methods.Run("tag unitTestTag"));
            Assert.AreEqual(true, Methods.Run("tag -d unitTestTag"));
        }
    }
}
