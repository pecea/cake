#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Files/bin/Debug/Files.dll"

new VoidJob("SearchFiles").Does(() => {
    GetFilesWithPattern(@"D:\Dane\Ernest\Praca\cake\Cake\scripts", @"*cript.csx");
});

JobManager.SetDefault("SearchFiles");
