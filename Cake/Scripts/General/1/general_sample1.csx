#r "C:\Users\Piotr\Source\Repos\cake\Cake\Build\bin\Debug\Build.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\NUnit\bin\Debug\NUnit.dll"
#r "C:\Users\Piotr\Source\Repos\cake\Cake\Zip\bin\Debug\Zip.dll"

using System;

new VoidJob("BuildProject")
    .Does(() => Build.Methods.BuildProjectAsync(@".\..\..\..\Success\Success.csproj").Wait());

new VoidJob("RunTests")
    .Does(() => 
    {
        if (!NUnit.Methods.RunTests(null, null, @".\..\..\..\Success\bin\Debug\Success.dll"))
            throw new ApplicationException("Tests failed");
    })
    .DependsOn("BuildProject");

new VoidJob("ZipResults")
    .Does(() => Zip.Methods.ZipFiles("results.zip", @".\..\..\..\Success\bin\Debug\"))
    .DependsOn("RunTests");

JobManager.SetDefault("ZipResults");
