#r "C:\Users\Piotr\Source\Repos\cake\Cake\Build\bin\Debug\Build.dll"

using Build;

new VoidJob("BuildProject").Does(() => Methods.BuildProjectAsync(@"C:\Users\Piotr\Source\Repos\local\TestApp\TestApp\TestLib\TestLib.csproj", @"C:\Users\Piotr\Desktop\test").Wait());

JobManager.SetDefault("BuildProject");
