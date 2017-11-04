#r "..\..\Build\bin\Debug\Build.dll"
#r "..\..\NUnit\bin\Debug\NUnit.dll"
#r "..\..\Zip\bin\Debug\Zip.dll"

new VoidJob("BuildProject")
    .Does(() => Build.Methods.BuildProjectAsync(@".\..\..\Fail\Fail.csproj").Wait());

new VoidJob("RunTests")
    .Does(() => NUnit.Methods.RunTests(null, null, @".\..\..\Fail\bin\Debug\Fail.dll"))
    .DependsOn("BuildProject");

new VoidJob("ZipResults")
    .Does(() => Zip.Methods.ZipFiles("results.zip", @".\..\..\Fail\bin\Debug\"))
    .DependsOn("RunTests");

JobManager.SetDefault("ZipResults");
