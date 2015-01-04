namespace Build.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class for testing Build module methods
    /// </summary>
    [TestClass]
    public class BuildMethodsTests
    {
        /// <summary>
        /// Test method for empty argument in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileArgumentIsEmpty()
        {
            Assert.AreEqual(false, Methods.BuildProject(""));
        }
        /// <summary>
        /// Test method for non-existing file in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileDoesNotExist()
        {
            Assert.AreEqual(false, Methods.BuildProject("Nonexisting or invalid project/solution file"));
        }
        /// <summary>
        /// Test method for invalid project file in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileIsNotAValidProjectFile()
        {
            Assert.AreEqual(false, Methods.BuildProject("Build.Tests.dll"));
        }
        /// <summary>
        /// Test method for successful build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfProjectIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
        }
        /// <summary>
        /// Test method for invalid platform in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "invalid platform"));
        }
        /// <summary>
        /// Test method for valid platform in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "Any CPU"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "x86"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", platform: "x64"));
        }
        /// <summary>
        /// Test method for invalid configuration in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../../Common/Common.csproj", configuration: "invalid configuration"));
        }
        /// <summary>
        /// Test method for valid configuration in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", configuration: "Debug"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj", configuration: "Release"));
        }
        /// <summary>
        /// Test method for invalid output path in build project method
        /// </summary>
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfOutputPathIsInvalid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../../Common/Common.csproj"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../../Common/Common.csproj", outputPath: "invalid output path?"));
        }
    }
}