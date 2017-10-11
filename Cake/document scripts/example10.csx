#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Zip/bin/Debug/Zip.dll"
#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Files/bin/Debug/Files.dll"

new VoidJob("SearchFiles").Does(() => {
    GetFilesWithPattern(@"D:\Dane\Ernest\Praca\TestOutput\", @"*");
});

new Job("ZipOutput").DependsOn("SearchFiles").Does(() => {
    return Zip.Methods.ZipFiles(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\TestOutput\");
});

new Job("ExtractOutput").DependsOn("ZipOutput").Does(() => {
    return Zip.Methods.ExtractFiles(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\Unzipped\");
});
new VoidJob("SearchUnzippedFiles").DependsOn("ExtractOutput").Does(() => {
    GetFilesWithPattern(@"D:\Dane\Ernest\Praca\Unzipped\TestOutput\", @"*");
});

JobManager.SetDefault("SearchUnzippedFiles");
