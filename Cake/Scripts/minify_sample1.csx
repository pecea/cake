// cake using "../../../Minify/bin/Debug/Minify.dll";

using System.IO;

new Job("MinifyCss")
    .Does(() => MinifyCss(
        pattern: PathManager.GetPath(@"**/*.css"),
        excludePattern: PathManager.GetPath(@"**/*.min.css")
    ));

new Job("MinifyJs")
    .Does(() => MinifyJs(
        pattern: PathManager.GetPath(@"**/*.js"),
        excludePattern: PathManager.GetPath(@"**/*.min.js")
    ));

new Job("BundleCss").DependsOn("MinifyCss")
    .Does(() => BundleFiles(
        pattern: PathManager.GetPath(@"**/*.min.css"), 
        destination: PathManager.GetPath("custom.min.css"),
        excludePattern: PathManager.GetPath("custom.min.css")
    ));

new Job("BundleJs").DependsOn("MinifyJs")
    .Does(() => BundleFiles(
        pattern: PathManager.GetPath(@"**/*.min.js"), 
        destination: PathManager.GetPath("custom.min.js"),
        excludePattern: PathManager.GetPath("custom.min.js")
    ));

JobManager.SetDefault(new VoidJob("Bundle").DependsOn("BundleCss", "BundleJs"));

static class PathManager
{
    const string BasePath = @"../../../Scripts/";

    public static string GetPath(string pathPart) 
    {
        return Path.Combine(BasePath, pathPart);
    }
}
