#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Files/bin/Debug/Files.dll"

new Job("ChangeText").Does(() => {
    System.Console.Write(System.IO.File.ReadAllText(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\script2.csx") + "\n");
    var result = ReplaceText(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\script2.csx", "Log", "newLog");

    System.Console.Write(System.IO.File.ReadAllText(@"D:\Dane\Ernest\Praca\cake\Cake\scripts\script2.csx") + "\n");
    return result;
});

JobManager.SetDefault("ChangeText");
