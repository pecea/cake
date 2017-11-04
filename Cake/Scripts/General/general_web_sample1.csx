#r "C:\Users\Piotr\Source\Repos\cake\Cake\Build\bin\Debug\Build.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\Minify\bin\Debug\Minify.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\Files\bin\Debug\Files.dll"

new VoidJob("Clean")
    .Does(() => Files.Methods.CleanDirectory(@".\Webroot\Publish"));

new VoidJob("MinifyJs")
    .Does(() => Minify.Methods.MinifyJs(pattern: @".\Webroot\*.js", excludePattern: @".\Webroot\*.min.js"))
    .DependsOn("Clean");

new VoidJob("MinifyCss")
    .Does(() => Minify.Methods.MinifyJs(pattern: @".\Webroot\*.css", excludePattern: @".\Webroot\*.min.css"))
    .DependsOn("Clean");

new VoidJob("BundleJs")
    .Does(() => Minify.Methods.BundleFiles(pattern: @".\Webroot\*.min.js", destination: @".\Webroot\Publish\custom.min.js"))
    .DependsOn("MinifyJs");

new VoidJob("BundleCss")
    .Does(() => Minify.Methods.BundleFiles(pattern: @".\Webroot\*.min.css", destination: @".\Webroot\Publish\custom.min.css"))
    .DependsOn("MinifyCss");

new VoidJob("Publish")
    .DependsOn("BundleCss", "BundleJs")
    .OnException("Clean");

JobManager.SetDefault("Publish");
