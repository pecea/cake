namespace NUnit.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    /// <summary>
    /// Test class for Nunit methods
    /// </summary>
    [TestClass]
    public class MsTestMethodsTests
    {
        //private const string PathToTestLibrary = @"../../../External/Success.dll";
        //private const string PathToFailingTestLibrary = @"../../../External/Fail.dll";
        private const string PathToTestLibrary = @"../../../Success/bin/debug/Success.dll";
        private const string PathToFailingTestLibrary = @"../../../Fail/bin/debug/Fail.dll";
        /// <summary>
        /// Test method for running Nunit tests
        /// </summary>
        [TestMethod]
        [TestCategory("NUnitMethods")]
        public void ShouldReturnSuccessIfValidTestPathIsSpecified()
        {
            Assert.IsTrue(Methods.RunTests(null, null, PathToTestLibrary));
        }
        /// <summary>
        /// Test method for running Nunit test with invalid path
        /// </summary>
        [TestMethod]
        [TestCategory("NUnitMethods")]
        public void ShouldReturnFailureIfInvalidTestPathIsSpecified()
        {
            Assert.IsFalse(Methods.RunTests(null, null, @"X:\Invalid\Path\To\tests.dll"));
        }
        /// <summary>
        /// Test method for running Nunit tests that are not passing
        /// </summary>
        [TestMethod]
        [TestCategory("NUnitMethods")]
        public void ShouldReturnFailureIfNotPassingTests()
        {
            Assert.IsFalse(Methods.RunTests(null, null, PathToFailingTestLibrary));
        }
        /// <summary>
        /// Test method for running only "first" category Nunit tests
        /// </summary>
        [TestMethod]
        [TestCategory("NUnitMethods")]
        public void ShouldRunOnlyFirstCategoryTests()
        {
            Assert.IsTrue(Methods.RunTests("cat == first", null, PathToTestLibrary));
        }
        /// <summary>
        /// Test method for running only "Priority = High" Nunit tests
        /// </summary>
        [TestMethod]
        [TestCategory("NUnitMethods")]
        public void ShouldRunOnlyHighPriorityTests()
        {
            Assert.IsTrue(Methods.RunTests("Priority == High", null, PathToTestLibrary));
        }
        //[TestMethod]
        //public void ShouldRunOnlyFirstCategoryTests()
        //{
        //    Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "first"));

        //}

        //public void ShouldRunOnlySecondCategoryTests()
        //{
        //    Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "second"));
        //}

        //public void ShouldRunOnlyTestsThatHaveFirstAndSecondCategory()
        //{
        //    Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "first&second"));
        //}

        //public void ShouldRunTestsFromFirstOrSecondCategory()
        //{
        //    Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "first|second"));
        //}

        //public void ShouldRunOnlyTestsWithOnlyFirstCategory()
        //{
        //    Methods.RunTests(PathToTestLibrary, "first&!second"); //first&!second?
        //}

        //public void ShouldNotRunAnyTests()
        //{
        //    Methods.RunTests(PathToTestLibrary, "third");
        //}

        //public void ShouldSaveResultsToFile()
        //{
        //    Methods.RunTests(PathToTestLibrary, null, "fileResult.trx");
        //}

        //public void ShouldSaveResultsToFileDifferentPath()
        //{
        //    Methods.RunTests(PathToTestLibrary, null, "../../fileResult.trx");
        //}

        //public void ShouldRunOnlyOneTest()
        //{
        //    Methods.RunTests(PathToTestLibrary, null, null, "TestMethod1");
        //}

        //public void ShouldRunSingleTests()
        //{
        //    Methods.RunTests(PathToTestLibrary, null, "TestMethod1, TestMethod2");
        //}

        //public void ShouldThrowErrorNotUniqueSingleTestName()
        //{
        //    Methods.RunTests(PathToTestLibrary, null, null, "TestMethod", true);
        //}

        //public void ShouldRunAllTests()
        //{
        //    Methods.RunTests(PathToTestLibrary, null, null, "TestMethod");
        //}
        ////test single test + multiple single tests + uniqueness
    }
}
