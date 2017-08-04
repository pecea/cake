using System.Linq;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Build.Tests
{
    /// <summary>
    /// Test class for build methods
    /// </summary>
    [TestClass]
    public class BuildMethodsTests
    {
        private const string ProjectPath = @"../../Test Files/Build.Tests.TestProject/Build.Tests.TestProject.csproj";
        private const string OutputPathDebug = @"../../Test Files/Build.Tests.TestProject/bin/Debug";
        private const string OutputPathRelease = @"../../Test Files/Build.Tests.TestProject/bin/Release";
        /// <summary>
        /// Initialize method to clean up previous builds
        /// </summary>
        [TestInitialize]
        public void CleanUpBuiltFiles()
        {
            Files.Methods.CleanDirectory(OutputPathDebug);
        }
        /// <summary>
        /// Test method for empty argument
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileArgumentIsEmpty()
        {
            Assert.IsFalse(Methods.BuildProject(""));
        }
        /// <summary>
        /// Test method for invalid argument
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileDoesNotExist()
        {
            Assert.IsFalse(Methods.BuildProject("Nonexisting or invalid project/solution file"));
        }
        /// <summary>
        /// Test method for invalid project file
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfProjectFileIsNotAValidProjectFile()
        {
            Assert.IsFalse(Methods.BuildProject("Build.Tests.dll"));
        }
        /// <summary>
        /// Test method for standard valid build
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfProjectIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
        }
        /// <summary>
        /// Test method for invalid platform
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, platform: "invalid platform"));
        }
        /// <summary>
        /// Test method for valid platform
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, platform: "x86"));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, platform: "x64"));
        }
        /// <summary>
        /// Test method for invalid configuration
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, configuration: "invalid configuration"));
        }
        /// <summary>
        /// Test method for valid configuration
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsTrue(Methods.BuildProject(ProjectPath, configuration: "Release"));
        }
        /// <summary>
        /// Test method for invalid output path
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldReturnFailureIfOutputPathIsInvalid()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            Assert.IsFalse(Methods.BuildProject(ProjectPath, "invalid output path?"));
        }
        /// <summary>
        /// test method for reassuring output was created
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldCreateFilesWhenBuildingAProject()
        {
            Files.Methods.CleanDirectory(OutputPathDebug);
            Assert.IsTrue(Methods.BuildProject(ProjectPath));

            Assert.IsTrue((OutputPathDebug + "/*.*").GetFilePaths().Any());
        }
        /// <summary>
        /// Test method for debug configuration
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public void BuildProjectShouldBuildAProjectInDebugIfDebugWasSpecified()
        {
            Assert.IsTrue(Methods.BuildProject(ProjectPath));
            using (var isolated = new Isolated<ConfigurationChecker>())
            {
                Assert.IsTrue(isolated.Value.IsDebug((OutputPathDebug + "/*.dll").GetFilePaths().First()));
            }
        }
        /// <summary>
        /// Test method for release configuration
        /// </summary>
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