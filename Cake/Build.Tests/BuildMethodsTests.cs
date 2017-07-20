using System.Linq;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Build.Tests
{
    [TestClass]
    public class BuildMethodsTests
    {
        private const string ProjectPath = @"../../Test Files/Build.Tests.TestProject/Build.Tests.TestProject.csproj";
        private const string OutputPathDebug = @"../../Test Files/Build.Tests.TestProject/bin/Debug";
        private const string OutputPathRelease = @"../../Test Files/Build.Tests.TestProject/bin/Release";

        [TestInitialize]
        public void CleanUpBuiltFiles()
        {
            Files.Methods.CleanDirectory(OutputPathDebug);
        }
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileArgumentIsEmpty()
        {
            Assert.IsFalse(Methods.BuildProject(""));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileDoesNotExist()
        {
            Assert.IsFalse(Methods.BuildProject("Nonexisting or invalid project/solution file"));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileIsNotAValidProjectFile()
        {
            Assert.IsFalse(Methods.BuildProject("Build.Tests.dll"));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfProjectIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
        }
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, platform: "invalid platform"));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, platform: "x86"));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, platform: "x64"));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, configuration: "invalid configuration"));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, configuration: "Release"));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfOutputPathIsInvalid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, outputPath: "invalid output path?"));
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldCreateFilesWhenBuildingAProject()
        {
            Files.Methods.CleanDirectory(OutputPathDebug);
            Assert.IsTrue(Methods.BuildProject(ProjectPath));

            Assert.IsTrue((OutputPathDebug + "/*.*").GetFilePaths().Any());
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldBuildAProjectInDebugIfDebugWasSpecified()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath, configuration: "Debug"));
            using (var isolated = new Isolated<ConfigurationChecker>())
            {
                Assert.IsTrue(isolated.Value.IsDebug((OutputPathDebug + "/*.dll").GetFilePaths().First()));
            }
        }

        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldBuildAProjectInReleaseIfReleaseWasSpecified()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath, configuration: "Release"));
            using (var isolated = new Isolated<ConfigurationChecker>())
            {
                Assert.IsFalse(isolated.Value.IsDebug((OutputPathRelease + "/*.dll").GetFilePaths().First()));
            }
        }
    }
}