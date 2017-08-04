// cake using "../../../Git/bin/Debug/Git.dll";

new Job("diff").Does(() => {
    Methods.RepositoryPath = @"C:\Users\Piotr Szyperski\Desktop\test repo";
    Methods.UserName = "Piotr Szyperski";
    Methods.UserEmail = "szyperski.p@gmail.com";
    //Methods.Commit("Test message!");
    Methods.Diff();
});

JobManager.SetDefault("diff");
