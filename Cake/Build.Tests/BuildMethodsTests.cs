namespace Build.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildMethodsTests
    {
        private const string ProjectPath = @"../../Test Files/Build.Tests.TestProject/Build.Tests.TestProject.csproj";

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
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath));
            Assert.AreEqual(false, Methods.BuildProject(ProjectPath, platform: "invalid platform"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath, platform: "Any CPU"));
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath, platform: "x86"));
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath, platform: "x64"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath));
            Assert.AreEqual(false, Methods.BuildProject(ProjectPath, configuration: "invalid configuration"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath, configuration: "Debug"));
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath, configuration: "Release"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfOutputPathIsInvalid()
        {
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath));
            Assert.AreEqual(false, Methods.BuildProject(ProjectPath, outputPath: "invalid output path?"));
        }

        //TODO: testowanie konfiguracji i czy pliki w folderach
    }
}