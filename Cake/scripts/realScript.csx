// cake using "../../../Build/bin/Debug/Build.dll";
// cake using "../../../NUnit/bin/Debug/NUnit.dll";
// cake using "../../../Zip/bin/Debug/Zip.dll";
// cake using "../../../Files/bin/Debug/Files.dll";
bool result;
new Job("BuildSolution").Does(() => {
    result = Build.Methods.BuildProject(@"D:\Dane\Ernest\Praca\cake\Cake\Cake.sln", @"D:\Dane\Ernest\Praca\TestOutput\", "Release");
	System.Console.WriteLine(result.ToString());
});
new Job("RunUnitTests").DependsOn("BuildSolution").Does(() => {
    result = NUnit.Methods.RunTests(null, null, @"D:\Dane\Ernest\Praca\cake\Cake\Success\bin\Debug\Success.dll");
	System.Console.WriteLine(result.ToString());
});
new Job("ZipOutput").DependsOn("RunUnitTests").Does(() => {
    result = Zip.Methods.ZipFiles(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\TestOutput\");
	System.Console.WriteLine(result.ToString());
});
new Job("CopyZip").DependsOn("ZipOutput").Does(() => {
    result = Files.Methods.CopyFile(@"D:\Dane\Ernest\Praca\ZippedOutput.zip", @"D:\Dane\Ernest\Praca\Deploy\ZippedOutput.zip");
	System.Console.WriteLine(result.ToString());
});
JobManager.SetDefault("CopyZip");