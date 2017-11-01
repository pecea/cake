using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            Files.Methods.CleanDirectory(OutputPathRelease);
        }
        /// <summary>
        /// Test method for empty argument
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnFailureIfProjectFileArgumentIsEmpty()
        {
            Assert.IsFalse(await Methods.BuildProjectAsync(""));
        }
        /// <summary>
        /// Test method for invalid argument
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnFailureIfProjectFileDoesNotExist()
        {
            Assert.IsFalse(await Methods.BuildProjectAsync("Nonexisting or invalid project/solution file"));
        }
        /// <summary>
        /// Test method for invalid project file
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task BuildProjectShouldThrowIfProjectFileIsNotAValidProjectFile()
        {
            await Methods.BuildProjectAsync("Build.Tests.dll");
        }
        /// <summary>
        /// Test method for standard valid build
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnSuccessIfProjectIsValid()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath));
        }
        /// <summary>
        /// Test method for invalid platform
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnFailureIfPlatformIsNotValid()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath));
            Assert.IsFalse(await Methods.BuildProjectAsync(ProjectPath, platform: "invalid platform"));
        }
        /// <summary>
        /// Test method for valid platform
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnSuccessIfPlatformIsValid()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath));
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath, platform: "x86"));
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath, platform: "x64"));
        }
        /// <summary>
        /// Test method for invalid configuration
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnFailureIfConfigurationIsNotValid()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath));
            Assert.IsFalse(await Methods.BuildProjectAsync(ProjectPath, configuration: "invalid configuration"));
        }
        /// <summary>
        /// Test method for valid configuration
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnSuccessIfConfigurationIsValid()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath));
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath, configuration: "Release"));
        }
        /// <summary>
        /// Test method for invalid output path
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldReturnFailureIfOutputPathIsInvalid()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath));
            Assert.IsFalse(await Methods.BuildProjectAsync(ProjectPath, "invalid: output path?"));
        }
        /// <summary>
        /// test method for reassuring output was created
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldCreateFilesWhenBuildingAProject()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath, OutputPathDebug));

            Assert.IsTrue((OutputPathDebug + "/*.*").GetFilePaths().Any());
        }
        /// <summary>
        /// Test method for debug configuration
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldBuildAProjectInDebugIfDebugWasSpecified()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath, OutputPathDebug));
            Assert.IsTrue(new ConfigurationChecker().IsDebug((OutputPathDebug + "/*.dll").GetFilePaths().First()));
        }
        /// <summary>
        /// Test method for release configuration
        /// </summary>
        [TestCategory("BuildMethods")]
        [TestMethod]
        public async Task BuildProjectShouldBuildAProjectInReleaseIfReleaseWasSpecified()
        {
            Assert.IsTrue(await Methods.BuildProjectAsync(ProjectPath, OutputPathRelease, "Release"));
            Assert.IsFalse(new ConfigurationChecker().IsDebug((OutputPathDebug + "/*.dll").GetFilePaths().First()));
        }
    }
}