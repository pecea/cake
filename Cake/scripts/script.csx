// cake using "../../../Minify/bin/Debug/Minify.dll";

new Job("Minification").Does(() => {
    return Methods.MinifyJs(@"D:\Dane\Ernest\js*", null, @"D:\Dane\Ernest\Praca\");
});
JobManager.SetDefault("Minification");
