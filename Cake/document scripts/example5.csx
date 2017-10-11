#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Build/bin/Debug/Build.dll"

new Job("BuildProject").Does(() => {
    return BuildProject(@"D:\Dane\Ernest\Praca\cake\Cake\Files\Files.csproj");
});

JobManager.SetDefault("BuildProject"); 