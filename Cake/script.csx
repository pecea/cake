// cake using "../../../Minify/bin/Debug/Minify.dll";

new Job("diff").Does(() =>
{
    //RepositoryPath = @"C:\Users\Piotr Szyperski\Desktop\test repo";
    //// UserIdentity = Identity.FromJsonFile(@"C:\Users\Piotr Szyperski\Desktop\creds.json");

    ////Pull();
    ////Fetch();
    ////Methods.UserName = "Piotr Szyperski";
    ////Methods.UserEmail = "szyperski.p@gmail.com";
    //////Methods.Commit("Test message!");
    //Logger.Log(LogLevel.Info, "elo");
    //DiffAll();
    ////Methods.Stage();
    ////Methods.Reset();
    ////Methods.Diff();
    //Methods.CommitAllChanges("elo3");
    //Methods.Push();
    //Methods.DiffAll();

    string basePath = @"C:\Users\Piotr Szyperski\Desktop\test repo\**\";
    string filesGlob = $"{basePath}*.js";
    string excludedGlob = $"{basePath}*.min.js";
    string destination = @"C:\Users\Piotr Szyperski\Desktop\test repo";

    MinifyJs(filesGlob, excludedGlob, destination);
});

JobManager.SetDefault("diff");
