using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Git.Tests
{
    [TestClass]
    public class GitMethodsTests
    {
        private static int _createdFilesCount;
        private static Commit _initialCommit;

        [ClassInitialize]
        public static void CreateRepository(TestContext context)
        {
            Methods.RepositoryPath = Path.Combine(context.TestRunResultsDirectory, "TestRepo");
            Methods.UserIdentity.Name = "Test User";
            Methods.UserIdentity.Email = "testuser@example.com";

            Repository.Init(Methods.RepositoryPath);
            MakeInitialCommit();
        }

        [TestInitialize]
        public void ResetRepository()
        {
            using (var repo = new Repository(Methods.RepositoryPath))
            {
                repo.Reset(ResetMode.Hard, _initialCommit);
                repo.RemoveUntrackedFiles();
            }
        }

        private static void MakeInitialCommit()
        {
            using (var repo = new Repository(Methods.RepositoryPath))
            {
                var signature = Methods.UserIdentity.GetSignature();
                _initialCommit = repo.Commit("Test initial commit", signature, signature);
            }
        }

        [TestMethod]
        [TestCategory("GitMethods")]
        public void AfterCommittingAllChangesShouldBeZeroWorkingDirChanges()
        {
            Assert.AreEqual(0, GetWorkingDirChangesCount());

            CreateFile();
            Assert.AreEqual(1, GetWorkingDirChangesCount());

            Methods.CommitAllChanges("Test commit");
            Assert.AreEqual(0, GetWorkingDirChangesCount());
        }

        [TestMethod]
        [TestCategory("GitMethods")]
        public void AfterStagingAFileShouldBeOneMoreIndexChange()
        {
            Assert.AreEqual(0, GetWorkingDirChangesCount());
            var filePath = CreateFile();
            Assert.AreEqual(1, GetWorkingDirChangesCount());

            var indexChanges = GetIndexChangesCount();
            Methods.Stage(filePath);
            Assert.AreEqual(indexChanges + 1, GetIndexChangesCount());
        }

        [TestMethod]
        [TestCategory("GitMethods")]
        public void AfterResetShouldBeZeroIndexChanges()
        {
            var filePath = CreateFile();

            Assert.AreEqual(0, GetIndexChangesCount());
            Methods.Stage(filePath);
            Assert.AreEqual(1, GetIndexChangesCount());
            Methods.ResetHard();
            Assert.AreEqual(0, GetIndexChangesCount());
        }

        private static int GetWorkingDirChangesCount() => GetChangesCount(DiffTargets.WorkingDirectory);

        private static int GetIndexChangesCount() => GetChangesCount(DiffTargets.Index);

        private static int GetChangesCount(DiffTargets diffTarget)
        {
            using (var repo = new Repository(Methods.RepositoryPath))
            {
                return repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, diffTarget).Count;
            }
        }

        private static string CreateFile()
        {
            var path = Path.Combine(Methods.RepositoryPath, $"test_file_{_createdFilesCount++}.txt");
            using (File.Create(path)) { }

            return path;
        }
    }
}