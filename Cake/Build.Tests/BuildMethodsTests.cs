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
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", platform: "invalid platform"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", platform: "Any CPU"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", platform: "x86"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", platform: "x64"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", configuration: "invalid configuration"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", configuration: "Debug"));
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", configuration: "Release"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfOutputPathIsInvalid()
        {
            Assert.AreEqual(true, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln"));
            Assert.AreEqual(false, Methods.BuildProject(@"../../Test Files/Test Project/Test Project.sln", outputPath: "invalid output path?"));
        }
    }
}