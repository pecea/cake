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

        [TestMethod]
        public void BuildProjectShouldCreateFilesWhenBuildingAProject()
        {
            Files.Methods.CleanDirectory(OutputPathDebug);
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath));

            Assert.AreEqual(true, (OutputPathDebug + "/*.*").GetFilePaths().Any());
        }

        [TestMethod]
        public void BuildProjectShouldBuildAProjectInDebugIfDebugWasSpecified()
        {
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath, configuration: "Debug"));
            using (var isolated = new Isolated<ConfigurationChecker>())
            {
                Assert.AreEqual(true, isolated.Value.IsDebug((OutputPathDebug + "/*.dll").GetFilePaths().First()));
            }
        }

        [TestMethod]
        public void BuildProjectShouldBuildAProjectInReleaseIfReleaseWasSpecified()
        {
            Assert.AreEqual(true, Methods.BuildProject(ProjectPath, configuration: "Release"));
            using (var isolated = new Isolated<ConfigurationChecker>())
            {
                Assert.AreEqual(true, isolated.Value.IsRelease((OutputPathRelease + "/*.dll").GetFilePaths().First()));
            }
        }
    }
}