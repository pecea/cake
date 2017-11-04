#r "..\..\Build\bin\Debug\Build.dll"
#r "..\..\NUnit\bin\Debug\NUnit.dll"
#r "..\..\Zip\bin\Debug\Zip.dll"

new VoidJob("BuildProject")
    .Does(() => Build.Methods.BuildProjectAsync(@".\..\..\Success\Success.csproj").Wait());

new VoidJob("RunTests")
    .Does(() => NUnit.Methods.RunTests(null, null, @".\..\..\Success\bin\Debug\Success.dll"))
    .DependsOn("BuildProject");

new VoidJob("ZipResults")
    .Does(() => Zip.Methods.ZipFiles("results.zip", @".\..\..\Success\bin\Debug\"))
    .DependsOn("RunTests");

JobManager.SetDefault("BuildProject");
