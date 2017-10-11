#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Build/bin/Debug/Build.dll"

new Job("BuildSolution").Does(() => {
    return BuildSolution(@"D:\Dane\Ernest\Praca\cake\Cake\Cake.sln");
});

JobManager.SetDefault("BuildSolution");
