#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Build/bin/Debug/Build.dll"

new Job("BuildProject").Does(() => {
    return BuildProject(@"D:\Dane\Ernest\Praca\cake\Cake\Fail\Fail.csproj");
});

JobManager.SetDefault("BuildProject");