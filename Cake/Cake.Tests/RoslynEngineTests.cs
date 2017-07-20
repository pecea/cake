using Common;

namespace Cake.Tests
{
    using System;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RoslynEngineTests
    {
        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("RoslynEngineMethods")]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ShouldThrowWhenTypeCannotBeFound()
        {
            using (var isolated = new Isolated<RoslynEngineFacade>())
            {
                isolated.Value.ExecuteFile(@"../../Test Files/ShouldThrowWhenTypeCannotBeFound.csx");
            }
        }

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

        [TestMethod]
        [TestCategory("CakeMethods")]
        [TestCategory("RoslynEngineMethods")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript()
        {
            using (var isolated = new Isolated<RoslynEngineFacade>())
            {
                isolated.Value.ExecuteFile(@"../../Test Files/ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript.csx");
            }
        }
    }
}