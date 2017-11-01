#r "C:\Thesis\Release\Files.dll"
#r "C:\Thesis\Release\Minify.dll"
using System.IO;

const string jsPath = @"C:\Thesis\Tests\MinifyTest\scripts";
const string cssPath = @"C:\Thesis\Tests\MinifyTest\styles";
const string htmlPath = @"C:\Thesis\Tests\MinifyTest\templates";
const string bundledPath = @"C:\Thesis\Tests\MinifyTest\bundled";
const string testPath = @"C:\Thesis\Tests";

var firstJob = new VoidJob("Structurize").Does(() =>
{
    Directory.CreateDirectory(jsPath);
    Directory.CreateDirectory(cssPath);
    Directory.CreateDirectory(htmlPath);
    Directory.CreateDirectory(bundledPath);

    var jsFiles = Files.Methods.GetFilesWithPattern(testPath, @"*.js", true);

    var cssFiles = Files.Methods.GetFilesWithPattern(testPath, @"*.css", true);

    var htmlFiles = Files.Methods.GetFilesWithPattern(testPath, @"*.html", true);

    foreach(var f in jsFiles)
    {
        Files.Methods.CopyFile(f, Path.Combine(jsPath, Path.GetFileName(f)));
    }
    foreach (var f in cssFiles)
    {
        Files.Methods.CopyFile(f, Path.Combine(cssPath, Path.GetFileName(f)));
    }
    foreach (var f in htmlFiles)
    {
        Files.Methods.CopyFile(f, Path.Combine(htmlPath, Path.GetFileName(f)));
    }
});

var exceptionJob = new VoidJob("Cleanup").Does(() =>
{
    Files.Methods.DeleteDirectoriesWithPattern(testPath, "MinifyTest", true);
});

var secondJob = new VoidJob("Minify").DependsOn(firstJob).Does(() =>
{
    var min1 = Minify.Methods.MinifyJs(
        pattern: Path.Combine(jsPath, @"**/*.js"),
        excludePattern: Path.Combine(jsPath, @"**/*.min.js"));
    if(!min1)
    {
        throw new System.ApplicationException("Javascript files not minified correctly!");
    }
    var min2 = Minify.Methods.MinifyCss(
        pattern: Path.Combine(cssPath, @"**/*.css"),
        excludePattern: Path.Combine(cssPath, @"**/*.min.css"));
    if(!min2)
        throw new System.ApplicationException("Cascading Style Sheets files not minified correctly!");
}).OnExcpetion("Cleanup");

var thirdJob = new Job("BundleFiles").DependsOn(firstJob, secondJob)
    .Does(() => {
        var bundle1 = Minify.Methods.BundleFiles(
        pattern: Path.Combine(jsPath, @"**/*.min.js"),
        destination: Path.Combine(bundledPath, "bundled.min.js")
        );
        if(!bundle1)
        {
            throw new System.ApplicationException("Javascript files not bundled correctly!");
        }
        var bundle2 = BundleFiles(
                pattern: Path.Combine(cssPath, @"**/*.min.css"),
                destination: Path.Combine(bundledPath, "bundled.min.css")
                );
        if (!bundle2)
            throw new System.ApplicationException("Cascading Style Sheets files not bundled correctly!");

        var bundle3 = BundleFiles(
        pattern: Path.Combine(htmlPath, @"**/*.html"),
        destination: Path.Combine(bundledPath, "bundled.html")
        );
        if (!bundle3)
            throw new System.ApplicationException("HyperText Markup Language files not bundled correctly!");

    }).OnException("Cleanup");

JobManager.SetDefault(thirdJob);