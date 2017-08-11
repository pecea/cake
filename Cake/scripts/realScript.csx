// cake using "../../../Build/bin/Debug/Build.dll";
// cake using "../../../NUnit/bin/Debug/NUnit.dll";
// cake using "../../../Zip/bin/Debug/Zip.dll";
// cake using "../../../Files/bin/Debug/Files.dll";

new Job("BuildSolution").Does(() => {
    return Build.Methods.BuildSolution(@"D:\Dane\Ernest\Praca\cake\Cake\Cake.sln", @"D:\Dane\Ernest\Praca\TestOutput\", "Release");
});
new Job("RunUnitTests").DependsOn("BuildSolution").Does(() => {
    return NUnit.Methods.RunTests(null, null, @"D:\Dane\Ernest\Praca\cake\Cake\Success\bin\Debug\Success.dll");
});
new Job("ZipOutput").DependsOn("RunUnitTests").Does(() => {
    return Zip.Methods.ZipFiles(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\TestOutput\");
});
new Job("CopyZip").DependsOn("ZipOutput").Does(() => {
    return Files.Methods.CopyFile(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\Deploy\ZippedOutput.zip");
});
JobManager.SetDefault("CopyZip");