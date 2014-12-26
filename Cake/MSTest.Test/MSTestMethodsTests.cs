namespace MSTest.Test
{
    using Methods;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MSTestMethodsTests
    {
        [TestMethod]
        public void ShouldReturnFailureIfInvalidTestPathIsSpecified()
        {
            Assert.AreEqual(false, Test(@"X:\Invalid\Path\To\tests.dll"));
        }

        [TestMethod]
        public void ShouldReturnSuccessIfValidTestPathIsSpecified()
        {
            Assert.AreEqual(true, Test(@"../../../Cake.Tests/bin/Debug/Cake.Tests.dll"));
        }

        [TestMethod]
        public void ShouldReturnFailureIfNotPassingTests()
        {
            Assert.AreEqual(false, Test(@"../../../Build.Tests/bin/Debug/Build.Tests.dll"));
        }
    }
}
