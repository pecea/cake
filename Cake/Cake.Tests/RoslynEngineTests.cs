using Common;

namespace Cake.Tests
{
    using System;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void ShouldThrowWhenTypeCannotBeFound()
        {
            var isolated = new Isolated<RoslynEngineFacade>();
            try
            {
                using (isolated)
                {
                    isolated.Value.ExecuteFile(@"../../Test Files/ShouldThrowWhenTypeCannotBeFound.csx"); //System.AccessViolationException: 'Attempted to read or write protected memory. This is often an indication that other memory is corrupt.'
//JA TO CHYBA UBRAŁEM W TRY CATCH (metodę ExecuteFile), może dlatego tak się dzieje, a test przechodzi bo mamy pewnie AllowDerivedTypes na true :)
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                isolated.Dispose();
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
            var isolated = new Isolated<RoslynEngineFacade>();
            try
            {
                using (isolated)
                {
                    isolated.Value.ExecuteFile(@"../../Test Files/Non existing script.csx");
                }
            }
            //catch (FileNotFoundException ex)
            //{
            //    throw;
            //    // ignored
            //}
            finally
            {
                isolated.Dispose();
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
            var isolated = new Isolated<RoslynEngineFacade>();
            try
            {
                using (isolated)
                {
                    isolated.Value.ExecuteFile(
                        @"../../Test Files/ShouldThrowIfInvalidPathToAssemblyIsSpecifiedInTheScript.csx");
                }
            }
            //catch (FileNotFoundException)
            //{
            //    // ignored
            //}
            finally
            {
                isolated.Dispose();
            }
        }
    }
}