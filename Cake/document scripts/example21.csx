#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Minify/bin/Debug/Minify.dll"
#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/NUnit/bin/Debug/NUnit.dll"
#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Git/bin/Debug/Git.dll"

new VoidJob("GitInit").Does(() => 
{
    Git.Methods.RepositoryPath = PathManager.RepositoryPath;
    Git.Methods.UserIdentity = Identity.FromJsonFile(@"C:\creds.json");
});
new Job("RunTests").Does(() => NUnit.Methods.RunTests(
    conditions: null, config: null,
    assemblyPaths: PathManager.RelativeToRepo("Tests.dll")));
new Job("Commit").DependsOn("RunTests", "GitInit").Does(() =>
{
    string input;
    Logger.Log(LogLevel.Info, "These changes will be committed:");
    Git.Methods.DiffWorkingDir();
    Logger.Log(LogLevel.Warn, "Commit these changes? (y/n)");

    do input = System.Console.ReadLine();
    while (input?.ToLower() != "y" && input?.ToLower() != "n");
    return input.ToLower() == "y" && Git.Methods.CommitAllChanges();
});
new Job("Pull").DependsOn("Commit").Does(Git.Methods.Pull);
new Job("Push").DependsOn("Pull").Does(Git.Methods.Push);
new Job("MinifyCss").DependsOn("Push")
    .Does(() => Minify.Methods.MinifyCss(
        PathManager.RelativeToRepo(@"**\*.css"),
        PathManager.RelativeToRepo(@"**\*.min.css")
    ));
new Job("MinifyJs").DependsOn("Push")
    .Does(() => Minify.Methods.MinifyJs(
        PathManager.RelativeToRepo(@"**\*.js"),
        PathManager.RelativeToRepo(@"**\*.min.js")
    ));
new Job("BundleCss").DependsOn("MinifyCss")
    .Does(() => Minify.Methods.BundleFiles(
        PathManager.RelativeToRepo(@"**\*.min.css"),
        PathManager.RelativeToRepo("custom.min.css"),
        excludePattern: PathManager.RelativeToRepo("custom.min.css")
    ));
new Job("BundleJs").DependsOn("MinifyJs")
    .Does(() => Minify.Methods.BundleFiles(
        PathManager.RelativeToRepo(@"**\*.min.js"),
        PathManager.RelativeToRepo("custom.min.js"),
        excludePattern: PathManager.RelativeToRepo("custom.min.js")
    ));
new VoidJob("Publish").DependsOn("BundleJs", "BundleCss");

JobManager.SetDefault("Publish");

static class PathManager
{
    public const string RepositoryPath = @"C:\Repository";

    public static string RelativeToRepo(string path) => System.IO.Path.Combine(RepostoryPath, path);
}
