namespace MSTest.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.VisualStudio.QualityTools;

    [TestClass]
    public class MsTestMethodsTests
    {
        //private const string PathToTestLibrary = @"../../Test Files/PassingTests.dll";
        private const string PathToTestLibrary = @"C:\Users\ErnestPrzestrzelski\Desktop\Ernest\priv\Backup\Praca Inzynierska\cake\Cake\MSTest.Tests\Test Files\PassingTests.dll";
        //private const string PathToTestLibrary = @"PassingTests.dll";
        private const string PathToFailingTestLibrary = @"../../Test Files/FailingTests.dll";
        [TestMethod]
        public void ShouldReturnSuccessIfValidTestPathIsSpecified()
        {
            Assert.IsTrue(Methods.RunTests(PathToTestLibrary));
        }

        [TestMethod]
        public void ShouldReturnFailureIfInvalidTestPathIsSpecified()
        {
            Assert.IsFalse(Methods.RunTests(@"X:\Invalid\Path\To\tests.dll"));
        }

        [TestMethod]
        public void ShouldReturnFailureIfNotPassingTests()
        {
            Assert.IsFalse(Methods.RunTests(PathToFailingTestLibrary));
        }

        [TestMethod]
        public void ShouldRunOnlyFirstCategoryTests()
        {
            Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "first"));

        }

        public void ShouldRunOnlySecondCategoryTests()
        {
            Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "second"));
        }

        public void ShouldRunOnlyTestsThatHaveFirstAndSecondCategory()
        {
            Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "first&second"));
        }

        public void ShouldRunTestsFromFirstOrSecondCategory()
        {
            Assert.IsTrue(Methods.RunTests(PathToTestLibrary, "first|second"));
        }

        public void ShouldRunOnlyTestsWithOnlyFirstCategory()
        {
            Methods.RunTests(PathToTestLibrary, "first&!second"); //first&!second?
        }

        public void ShouldNotRunAnyTests()
        {
            Methods.RunTests(PathToTestLibrary, "third");
        }

        public void ShouldSaveResultsToFile()
        {
            Methods.RunTests(PathToTestLibrary, null, "fileResult.trx");
        }

        public void ShouldSaveResultsToFileDifferentPath()
        {
            Methods.RunTests(PathToTestLibrary, null, "../../fileResult.trx");
        }

        public void ShouldRunOnlyOneTest()
        {
            Methods.RunTests(PathToTestLibrary, null, null, "TestMethod1");
        }

        public void ShouldRunSingleTests()
        {
            Methods.RunTests(PathToTestLibrary, null, "TestMethod1, TestMethod2");
        }

        public void ShouldThrowErrorNotUniqueSingleTestName()
        {
            Methods.RunTests(PathToTestLibrary, null, null, "TestMethod", true);
        }

        public void ShouldRunAllTests()
        {
            Methods.RunTests(PathToTestLibrary, null, null, "TestMethod");
        }
        //test single test + multiple single tests + uniqueness
    }
}
