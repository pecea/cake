#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Build/bin/Debug/Build.dll"
#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/NUnit/bin/Debug/NUnit.dll"
#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Zip/bin/Debug/Zip.dll"
#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Files/bin/Debug/Files.dll"
#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Minify/bin/Debug/Minify.dll"

new Job("BuildSolution").Does(() => {
    return Build.Methods.BuildSolution(@"D:\Dane\Ernest\Praca\cake\Cake\Cake.sln", @"D:\Dane\Ernest\Praca\TestOutput\", "Release");
});
new Job("MinifyJs").DependsOn("BuildSolution").Does(() => {
    return Minify.Methods.MinifyJs(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\js*", null, @"D:\Dane\Ernest\Praca\TestOutput\");
});
new Job("BundleJs").DependsOn("MinifyJs").Does(() => {
    return Minify.Methods.BundleFiles(@"D:\Dane\Ernest\Praca\TestOutput\*min.js", @"D:\Dane\Ernest\Praca\TestOutput\bundled.min.js", ';');
});
new Job("BundleHtml").DependsOn("BundleJs").Does(() => {
    return Minify.Methods.BundleFiles(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\*html", @"D:\Dane\Ernest\Praca\TestOutput\bundled.html", '\n');
});
new Job("RunUnitTests").DependsOn("BundleJs").Does(() => {
    return NUnit.Methods.RunTests(null, null, @"D:\Dane\Ernest\Praca\cake\Cake\Success\bin\Debug\Success.dll");
});
new Job("ZipOutput").DependsOn("RunUnitTests").Does(() => {
    return Zip.Methods.ZipFiles(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\TestOutput\");
});
new Job("CopyZip").DependsOn("ZipOutput").Does(() => {
    return Files.Methods.CopyFile(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\Deploy\ZippedOutput.zip");
});
JobManager.SetDefault("CopyZip");
