using System.IO;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cake.Tests
{
    /// <summary>
    /// Test class for RoslynEngine tests
    /// </summary>
    [TestClass]
    public class RoslynEngineTests
    {
        /// <summary>
        /// Test method for unknown type in the script
        /// </summary>
        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("RoslynEngineMethods")]
        [ExpectedException(typeof(JobException))]
        public void ShouldThrowWhenTypeCannotBeFound()
        {
            using (var isolated = new Isolated<RoslynEngineFacade>())
            {
                isolated.Value.ExecuteFile(@"../../Test Files/ShouldThrowWhenTypeCannotBeFound.csx");
            }
        }
        /// <summary>
        /// Test method for non-existing script
        /// </summary>
        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("RoslynEngineMethods")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowWhenNonExistingScriptIsSpecified()
        {
            using (var isolated = new Isolated<RoslynEngineFacade>())
            {
                isolated.Value.ExecuteFile(@"../../Test Files/Non existing script.csx");
            }
        }
        /// <summary>
        /// Test method for invalid reference in the script
        /// </summary>
        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("RoslynEngineMethods")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript()
        {
            using (var isolated = new Isolated<RoslynEngineFacade>())
            {
                isolated.Value.ExecuteFile(
                    @"../../Test Files/ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript.csx");
            }
        }
    }
}