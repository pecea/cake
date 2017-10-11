#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/XUnit/bin/Debug/XUnit.dll"

new Job("UnitTests").Does(() => {
    return Methods.RunTests(null, null, @"D:\Dane\Ernest\Praca\cake\Cake\Success2\bin\Debug\Success2.dll");
});
JobManager.SetDefault("UnitTests");
