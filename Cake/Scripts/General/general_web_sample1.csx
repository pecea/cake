#r "C:\Users\Piotr\Source\Repos\cake\Cake\Build\bin\Debug\Build.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\Minify\bin\Debug\Minify.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\Files\bin\Debug\Files.dll"

using System;

new VoidJob("Clean")
    .Does(() =>
    {
        if (!Files.Methods.CleanDirectory(@".\Webroot\Publish"))
            throw new ApplicationException("Clean failed");
    });

new VoidJob("MinifyJs")
    .Does(() =>
    {
        if (!Minify.Methods.MinifyJs(pattern: @".\Webroot\*.js", excludePattern: @".\Webroot\*.min.js"))
            throw new ApplicationException("Minify js failed");
    })
    .DependsOn("Clean");

new VoidJob("MinifyCss")
    .Does(() =>
    {
        if (!Minify.Methods.MinifyJs(pattern: @".\Webroot\*.css", excludePattern: @".\Webroot\*.min.css"))
            throw new ApplicationException("Minify css failed");
    })
    .DependsOn("Clean");

new VoidJob("BundleJs")
    .Does(() =>
    {
        if (!Minify.Methods.BundleFiles(pattern: @".\Webroot\*.min.js", destination: @".\Webroot\Publish\custom.min.js"))
            throw new ApplicationException("Bundle js failed");
    })
    .DependsOn("MinifyJs");

new VoidJob("BundleCss")
    .Does(() =>
    {
        if (!Minify.Methods.BundleFiles(pattern: @".\Webroot\*.min.css", destination: @".\Webroot\Publish\custom.min.css"))
            throw new ApplicationException("Bundle css failed");
    })
    .DependsOn("MinifyCss");

new VoidJob("Publish")
    .DependsOn("BundleCss", "BundleJs")
    .OnException("Clean");

JobManager.SetDefault("Publish");
