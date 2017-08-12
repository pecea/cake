using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Git.Tests
{
    [TestClass]
    public class GitMethodsTests
    {
        const string Path = @"..\..\Test Files\UnitTestRepository\";
        
        //Commit
        //Diff
        //Fetch
        //Pull
        //Push
        //Reset
        //Stage



        //[TestInitialize]
        //public void CreateRepository()
        //{
        //    //Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());
        //    //Methods.PathToRepository = Directory.GetCurrentDirectory();
        //    if (!Directory.Exists(Path))
        //        Directory.CreateDirectory(Path);
        //    Directory.SetCurrentDirectory(Path);
        //    Methods.Run("init");
        //    Methods.Run("add");
        //    //Methods.Run("commit");
        //}

        //[TestMethod]
        //public void TagShouldReturnSuccess()
        //{
        //    Directory.SetCurrentDirectory(Path);
        //    Assert.IsTrue(Methods.Tag("unitTestTagut"));
        //    Assert.IsTrue(Methods.Tag("-d unitTestTagut"));
        //}

        //[TestMethod]
        //public void CleanShouldReturnSuccess()
        //{
        //    Directory.SetCurrentDirectory(Path);
        //    Assert.IsTrue(Methods.Clean());
        //}

        //[TestMethod]
        //public void RunShouldReturnSuccess()
        //{
        //    Directory.SetCurrentDirectory(Path);
        //    Assert.IsTrue(Methods.Run("tag unitTestTagut"));
        //    Assert.IsTrue(Methods.Run("tag -d unitTestTagut"));
        //}

        //[TestMethod]
        //public void CurrentBranchShouldReturnSuccess()
        //{
        //    Directory.SetCurrentDirectory(Path);
        //    Assert.IsTrue(Methods.CurrentBranch());
        //}

        //[TestMethod]
        //public void CurrentShaShouldReturnSuccess()
        //{
        //    Directory.SetCurrentDirectory(Path);
        //    Assert.IsTrue(Methods.CurrentSha());
        //}

        //[TestCleanup]
        //public void CleanUp()
        //{
        //    try
        //    {
        //        Directory.SetCurrentDirectory(Path);
        //        Methods.Clean(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine(ex);
        //        throw;
        //    }
        //}
    }
}
