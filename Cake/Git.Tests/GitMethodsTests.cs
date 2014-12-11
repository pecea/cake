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
            Methods.PathToExe = @"C:\Users\ernes_000\Source\Repos\Praca Inżynierska\Cake\Git.Tests\bin\Debug\testForCake";
            Assert.AreEqual(true, Methods.Tag("unitTestTag"));
            Assert.AreEqual(true, Methods.Tag("-d unitTestTag"));
        }

        [TestMethod]
        public void ResetAllModificationsShouldReturnSucces()
        {
            Assert.AreEqual(true, Methods.ResetAllModifications());
        }

        [TestMethod]
        public void PushShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.Push(@"C:\Users\ernes_000\Source\Repos\Praca Inżynierska\Cake\Git.Tests\bin\Debug\testForCake"));
        }

        [TestMethod]
        public void CleanShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.Clean());
        }

        [TestMethod]
        public void RunShouldReturnSuccess()
        {
            Assert.AreEqual(true, Methods.Run("tag unitTestTag"));
            Assert.AreEqual(true, Methods.Run("tag -d unitTestTag"));
        }
    }
}
