#r "C:\Users\Piotr\Source\Repos\cake\Cake\Build\bin\Debug\Build.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\NUnit\bin\Debug\NUnit.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\Zip\bin\Debug\Zip.dll"

using System;

new VoidJob("BuildProject")
    .Does(() => 
    {
        if (!Build.Methods.BuildProjectAsync(@".\..\..\Fail\Fail.csproj").Result)
            throw new ApplicationException("Build failed");
    });

new VoidJob("RunTests")
    .Does(() => 
    {
        if (!NUnit.Methods.RunTests(null, null, @".\..\..\Fail\bin\Debug\Fail.dll"))
            throw new ApplicationException("Tests failed");
    })
    .DependsOn("BuildProject");

new VoidJob("ZipResults")
    .Does(() => Zip.Methods.ZipFiles("results.zip", @".\..\..\Fail\bin\Debug\"))
    .DependsOn("RunTests");

JobManager.SetDefault("ZipResults");
