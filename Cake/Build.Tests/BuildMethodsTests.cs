namespace Build.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildMethodsTests
    {
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileArgumentIsEmpty()
        {
            Assert.AreEqual(false, Methods.BuildProject(""));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileDoesNotExist()
        {
            Assert.AreEqual(false, Methods.BuildProject("Nonexisting or invalid project/solution file"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileIsNotAValidProjectFile()
        {
            Assert.AreEqual(false, Methods.BuildProject("Build.Tests.dll"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfProjectIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "invalid platform"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "Any CPU"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "x86"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "x64"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../../Common/Common.csproj", configuration: "invalid configuration"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", configuration: "Debug"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", configuration: "Release"));
        }

        [TestMethod]
        public void BuildProjectShouldReturFailureIfOutputPathIsInvalid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../../Common/Common.csproj", outputPath: "invalid output path?"));
        }
    }
}
