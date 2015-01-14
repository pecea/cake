namespace MSTest.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MsTestMethodsTests
    {
        private const string PathToTestLibrary = @"../../Test Files/SampleUnitTestProject.dll";
        private const string PathToFailingTestLibrary = @"../../Test Files/SampleFailingUnitTestProject.dll";

        [TestMethod]
        public void ShouldReturnFailureIfInvalidTestPathIsSpecified()
        {
            Assert.AreEqual(false, Methods.Test(@"X:\Invalid\Path\To\tests.dll"));
        }

        [TestMethod]
        public void ShouldReturnSuccessIfValidTestPathIsSpecified()
        {
            Assert.AreEqual(true, Methods.Test(PathToTestLibrary));
        }

        [TestMethod]
        public void ShouldReturnFailureIfNotPassingTests()
        {
            Assert.AreEqual(false, Methods.Test(PathToFailingTestLibrary));
        }
    }
}
