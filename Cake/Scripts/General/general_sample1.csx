#r "..\..\Build\bin\Debug\Build.dll"
#r "..\..\NUnit\bin\Debug\NUnit.dll"
#r "..\..\Zip\bin\Debug\Zip.dll"

string projPath = @".\TestApp\TestLib\";
//string projPath = @".\..\..\Success\";
string projName = "TestLib";
//string projName = "Success";

new VoidJob("BuildProject")
    .Does(() => Build.Methods.BuildProjectAsync($"{projPath}{projName}.csproj").Wait());

new VoidJob("RunTests")
    .Does(() => NUnit.Methods.RunTests(null, null, $@"{projPath}bin\Debug\{projName}.dll"))
    .DependsOn("BuildProject");

new VoidJob("ZipResults")
    .Does(() => Zip.Methods.ZipFiles("results.zip", $@"{projPath}bin\Debug\"))
    .DependsOn("RunTests");

JobManager.SetDefault("ZipResults");
