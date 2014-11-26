namespace Build.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildMethodsTests
    {
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileArgumentIsEmpty()
        {
            Assert.AreEqual(1, Methods.BuildProject(""));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileDoesNotExist()
        {
            Assert.AreEqual(1, Methods.BuildProject("Nonexisting or invalid project/solution file"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileIsNotAValidProjectFile()
        {
            Assert.AreEqual(1, Methods.BuildProject("Build.Tests.dll"));
        }
    }
}
