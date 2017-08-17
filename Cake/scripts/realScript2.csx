// cake using "../../../Minify/bin/Debug/Minify.dll";
// cake using "../../../NUnit/bin/Debug/NUnit.dll";
// cake using "../../../Git/bin/Debug/Git.dll";

using System;
using System.IO;

new VoidJob("GitInit").Does(() =>
{
    RepositoryPath = PathManager.RepositoryPath;
    UserIdentity = Identity.FromJsonFile(@"C:\creds.json");
});

new Job("RunTests").Does(() => NUnit.Methods.RunTests(null, null, PathManager.RelativeToRepo("Success.dll")));

new Job("Commit").DependsOn("RunTests", "GitInit").Does(() =>
{
    string input;

    Logger.Log(LogLevel.Info, "These changes will be committed:");
    DiffWorkingDir();

    Logger.Log(LogLevel.Warn, "Commit these changes? (y/n)");

    do input = Console.ReadLine();
    while (input.ToLower() != "y" && input.ToLower() != "n");

    return input.ToLower() == "y"
        ? CommitAllChanges()
        : false;
});

new Job("Pull").DependsOn("Commit").Does(Pull);

new Job("Push").DependsOn("Pull").Does(Push);

new Job("MinifyCss").DependsOn("Push")
    .Does(() => MinifyCss(
        pattern: PathManager.RelativeToRepo(@"**\*.css"),
        excludePattern: PathManager.RelativeToRepo(@"**\*.min.css")
    ));

new Job("MinifyJs").DependsOn("Push")
    .Does(() => MinifyJs(
        pattern: PathManager.RelativeToRepo(@"**\*.js"),
        excludePattern: PathManager.RelativeToRepo(@"**\*.min.js")
    ));

new Job("BundleCss").DependsOn("MinifyCss")
    .Does(() => BundleFiles(
        pattern: PathManager.RelativeToRepo(@"**\*.min.css"), 
        destination: PathManager.RelativeToRepo("custom.min.css"),
        excludePattern: PathManager.RelativeToRepo("custom.min.css")
    ));

new Job("BundleJs").DependsOn("MinifyJs")
    .Does(() => BundleFiles(
        pattern: PathManager.RelativeToRepo(@"**\*.min.js"), 
        destination: PathManager.RelativeToRepo("custom.min.js"),
        excludePattern: PathManager.RelativeToRepo("custom.min.js")
    ));

new VoidJob("Publish").DependsOn("BundleJs", "BundleCss");

JobManager.SetDefault("Publish");

static class PathManager
{
    public const string RepositoryPath = @"C:\Repository";

    public static string RelativeToRepo(string path)
    {
        return Path.Combine(RepositoryPath, path);
    }
}