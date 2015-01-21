namespace Cake.Tests
{
    using System;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RoslynEngineTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ShouldThrowWhenTypeCannotBeFound()
        {
            RoslynEngine.ExecuteFile(@"../../Test Files/ShouldThrowWhenTypeCannotBeFound.csx");

            // BUG: Access violation exception
            /*using (var isolated = new Isolated<RoslynEngineFacade>())
            {
                isolated.Value.ExecuteFile(@"../../Test Files/ShouldThrowWhenTypeCannotBeFound.csx");
            }*/
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowWhenNotExistingScriptIsSpecified()
        {
            RoslynEngine.ExecuteFile(@"../../Test Files/Non existing script.csx");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript()
        {
            RoslynEngine.ExecuteFile(@"../../Test Files/ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript.csx");
        }

        //TODO: przerobić na isolated i coś jeszcze jak w duszy zagra
    }
}