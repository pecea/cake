using System.IO;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.CodeAnalysis.Scripting;

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
        public void ShouldThrowWhenTypeCannotBeFound()
        {
            try
            {
                RoslynEngine.Instance.ExecuteFile(@"../../Test Files/ShouldThrowWhenTypeCannotBeFound.csx").Wait();
            }
            catch (AggregateException e)
            {
                Assert.IsTrue(e.InnerException is CompilationErrorException);
                return;
            }

            Assert.IsTrue(false);
        }

        /// <summary>
        /// Test method for non-existing script
        /// </summary>
        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("RoslynEngineMethods")]
        public void ShouldThrowWhenNonExistingScriptIsSpecified()
        {
            try
            {
                RoslynEngine.Instance.ExecuteFile(@"../../Test Files/Non existing script.csx").Wait();
            }
            catch (AggregateException e)
            {
                Assert.IsTrue(e.InnerException is FileNotFoundException);
                return;
            }

            Assert.IsTrue(false);
        }
        /// <summary>
        /// Test method for invalid reference in the script
        /// </summary>
        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("RoslynEngineMethods")]
        public void ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript()
        {
            try
            {
                RoslynEngine.Instance.ExecuteFile(@"../../Test Files/ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript.csx").Wait();
            }
            catch (AggregateException e)
            {
                Assert.IsTrue(e.InnerException is CompilationErrorException);
                return;
            }

            Assert.IsTrue(false);
        }
    }
}