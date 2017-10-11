#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Zip/bin/Debug/Zip.dll"
new Job("ZipOutput").Does(() => {
    return Zip.Methods.ZipFilesWithOptions(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", "password", 
        "fastest", false, false, @"D:\Dane\Ernest\Praca\TestOutput\");
});

new Job("ExtractOutput").DependsOn("ZipOutput").Does(() => {
    return Zip.Methods.ExtractFiles(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\TestOutput\", "password");
});
JobManager.SetDefault("ExtractOutput");
