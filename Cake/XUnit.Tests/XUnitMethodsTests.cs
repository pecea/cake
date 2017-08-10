using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XUnit.Tests
{
    [TestClass]
    public class XUnitMethodsTests
    {
        private const string PathToTestLibrary = @"../../../Success2/bin/debug/Success2.dll";
        private const string PathToFailingTestLibrary = @"../../../Fail2/bin/debug/Fail2.dll";

        /// <summary>
        /// Test method for running Nunit tests
        /// </summary>
        [TestMethod]
        [TestCategory("XUnitMethods")]
        public void ShouldReturnSuccessIfValidTestPathIsSpecified()
        {
            Assert.IsTrue(Methods.RunTests(null, null, PathToTestLibrary));
        }

        /// <summary>
        /// Test method for running Nunit test with invalid path
        /// </summary>
        [TestMethod]
        [TestCategory("XUnitMethods")]
        public void ShouldReturnFailureIfInvalidTestPathIsSpecified()
        {
            Assert.IsFalse(Methods.RunTests(null, null, @"X:\Invalid\Path\To\tests.dll"));
        }
        /// <summary>
        /// Test method for running Nunit tests that are not passing
        /// </summary>
        [TestMethod]
        [TestCategory("XUnitMethods")]
        public void ShouldReturnFailureIfNotPassingTests()
        {
            Assert.IsFalse(Methods.RunTests(null, null, PathToFailingTestLibrary));
        }
        /// <summary>
        /// Test method for running only "first" category Nunit tests
        /// </summary>
        [TestMethod]
        [TestCategory("XUnitMethods")]
        public void ShouldRunOnlyFirstCategoryTests()
        {
            Assert.IsTrue(Methods.RunTests("Category=first", null, PathToTestLibrary));//, "-trait ""Category=first"));
        }
    }
}
