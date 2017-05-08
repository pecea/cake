using System.Linq;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Build.Tests
{
    [TestClass]
    internal class BuildMethodsTests
    {
        private const string ProjectPath = @"../../Test Files/Build.Tests.TestProject/Build.Tests.TestProject.csproj";
        private const string OutputPathDebug = @"../../Test Files/Build.Tests.TestProject/bin/Debug";
        private const string OutputPathRelease = @"../../Test Files/Build.Tests.TestProject/bin/Release";

        [TestInitialize]
        public void CleanUpBuiltFiles()
        {
            Files.Methods.CleanDirectory(OutputPathDebug);
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileArgumentIsEmpty()
        {
            Assert.IsFalse(Methods.BuildProject(""));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileDoesNotExist()
        {
            Assert.IsFalse(Methods.BuildProject("Nonexisting or invalid project/solution file"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileIsNotAValidProjectFile()
        {
            Assert.IsFalse(Methods.BuildProject("Build.Tests.dll"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfProjectIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, platform: "invalid platform"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, platform: "x86"));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, platform: "x64"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, configuration: "invalid configuration"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, configuration: "Release"));
        }

        [TestMethod]
        public void BuildProjectShouldReturnFailureIfOutputPathIsInvalid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, outputPath: "invalid output path?"));
        }

        [TestMethod]
        public void BuildProjectShouldCreateFilesWhenBuildingAProject()
        {
            Files.Methods.CleanDirectory(OutputPathDebug);
            Assert.IsTrue(Methods.BuildProject(ProjectPath));

            Assert.IsTrue((OutputPathDebug + "/*.*").GetFilePaths().Any());
        }

        [TestMethod]
        public void BuildProjectShouldBuildAProjectInDebugIfDebugWasSpecified()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            using (var isolated = new Isolated<ConfigurationChecker>())
            {
                Assert.IsTrue(isolated.Value.IsDebug((OutputPathDebug + "/*.dll").GetFilePaths().First()));
            }
        }

        [TestMethod]
        public void BuildProjectShouldBuildAProjectInReleaseIfReleaseWasSpecified()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath, configuration: "Release"));
            using (var isolated = new Isolated<ConfigurationChecker>())
            {
                Assert.IsTrue(isolated.Value.IsRelease((OutputPathRelease + "/*.dll").GetFilePaths().First()));
            }
        }
    }
}