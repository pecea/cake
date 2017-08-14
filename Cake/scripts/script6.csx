// cake using "../../../Build/bin/Debug/Build.dll";
			
var t = new Job("BuildProject").Does(() =>
{
    var res = Build.Methods.BuildProject(@"D:\Dane\Ernest\Praca\cake\Cake\Files\Files.csproj", @"D:\Dane\Ernest\Praca\TestOutput\", "Release");
    return res;
});
JobManager.SetDefault("BuildProject");