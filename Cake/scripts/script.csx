// cake using "../../../Minify/bin/Debug/Minify.dll";

new Job("MinifyJs").Does(() => {
    return Minify.Methods.MinifyJs(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\js*", null, @"D:\Dane\Ernest\Praca\TestOutput\");
});
new Job("BundleJs").DependsOn("MinifyJs").Does(() => {
    return Minify.Methods.BundleFiles(@"D:\Dane\Ernest\Praca\TestOutput\*min.js", ';', @"D:\Dane\Ernest\Praca\TestOutput\bundled.min.js");
});