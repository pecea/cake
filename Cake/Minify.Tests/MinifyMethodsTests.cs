using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Minify.Tests
{
    [TestClass]
    public class MinifyMethodsTests
    {
        private const string PathForTests = "../../Test Files/";

        [TestInitialize]
        public void RemoveMinifiedAndBundledFiles()
        {
            foreach(var file in Files.Methods.GetFilesWithPattern($"{PathForTests}Minified/", "*"))
                File.Delete(file);
            foreach (var file in Files.Methods.GetFilesWithPattern($"{PathForTests}Bundled/", "*"))
                File.Delete(file);
        }

        [TestMethod]
        [TestCategory("MinifyMethods")]
        public void MinifyJsShouldMinifyFilesSuccesfully()
        {
            Assert.IsTrue(Methods.MinifyJs($"{PathForTests}*js", $"{PathForTests}syntaxError*", $"{PathForTests}Minified/"));
            var files = Files.Methods.GetFilesWithPattern(PathForTests, "*js").Where(f => !f.Contains("syntaxError"));
            var minFiles = Files.Methods.GetFilesWithPattern($"{PathForTests}Minified/", "*min.js");
            var fileDictionary = new Dictionary<string, string>();
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file) ?? string.Empty;
                var minFile = minFiles.FirstOrDefault(m => m.Contains(name));
                fileDictionary.Add(file, minFile);
            }
            foreach (var entry in fileDictionary)
            {
                Assert.IsTrue(File.ReadAllText(entry.Key).Length >= File.ReadAllText(entry.Value).Length);
            }
        }

        [TestMethod]
        [TestCategory("MinifyMethods")]
        public void MinifyCssShouldMinifyFilesSuccesfully()
        {
            Assert.IsTrue(Methods.MinifyJs($"{PathForTests}*css", null, $"{PathForTests}Minified/"));
            var files = Files.Methods.GetFilesWithPattern(PathForTests, "*css");
            var minFiles = Files.Methods.GetFilesWithPattern($"{PathForTests}Minified/", "*min.css");
            var fileDictionary = new Dictionary<string, string>();
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file) ?? string.Empty;
                var minFile = minFiles.FirstOrDefault(m => m.Contains(name));
                fileDictionary.Add(file, minFile);
            }
            foreach (var entry in fileDictionary)
            {
                Assert.IsTrue(File.ReadAllText(entry.Key).Length >= File.ReadAllText(entry.Value).Length);
            }
        }

        [TestMethod]
        [TestCategory("MinifyMethods")]
        public void MinifyJsShouldDoSomethingWhenErrorInFile()
        {
            Assert.IsTrue(Methods.MinifyJs($"{PathForTests}syntaxError.js", null, $"{PathForTests}Minified/"));
        }

        [TestMethod]
        [TestCategory("MinifyMethods")]
        public void MinifyJsShouldReturnFalseIfNoFileMatcheesPattern()
        {
            Assert.IsFalse(Methods.MinifyJs($"{PathForTests}*.ts", $"{PathForTests}Minified/"));
        }

        //[TestMethod]
        //[TestCategory("MinifyMethods")]
        //public void MinifyCssShouldDoSomethingWhenErrorInFile()
        //{
        //}

        [TestMethod]
        [TestCategory("MinifyMethods")]
        public void BundleFilesShouldBundleIfParametersAreValid()
        {
            Assert.IsTrue(Methods.MinifyJs($"{PathForTests}*js", $"{PathForTests}syntaxError*", $"{PathForTests}Minified/"));
            Assert.IsTrue(Methods.BundleFiles($"{PathForTests}Minified/*min.js", $"{PathForTests}Bundled/bundled.min.js", ';'));
            Assert.IsTrue(File.Exists($"{PathForTests}Bundled/bundled.min.js"));

        }

        [TestMethod]
        [TestCategory("MinifyMethods")]
        public void BundleFilesShouldReturnFalseIfEmptyDestination()
        {
            Assert.IsFalse(Methods.BundleFiles($"{PathForTests}*min.js", string.Empty, ';'));
        }

        //MinifyCss
        //MinifyJs
        //BundleFiles
    }
}
