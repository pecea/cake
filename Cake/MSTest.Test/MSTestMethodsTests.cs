namespace MSTest.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MsTestMethodsTests
    {
        [TestMethod]
        public void ShouldReturnFailureIfInvalidTestPathIsSpecified()
        {
            Assert.AreEqual(false, Methods.Test(@"X:\Invalid\Path\To\tests.dll"));
        }

        [TestMethod]
        public void ShouldReturnSuccessIfValidTestPathIsSpecified()
        {
            Assert.AreEqual(true, Methods.Test(@"../../../Cake.Tests/bin/Debug/Cake.Tests.dll"));
        }

        [TestMethod]
        public void ShouldReturnFailureIfNotPassingTests()
        {
            Assert.AreEqual(false, Methods.Test(@"../../../Build.Tests/bin/Debug/Build.Tests.dll"));
        }
    }
}
