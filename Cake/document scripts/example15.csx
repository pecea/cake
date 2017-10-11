#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/NUnit/bin/Debug/NUnit.dll"

new Job("UnitTests").Does(() => {
    return Methods.RunTests(null, null, @"D:\Dane\Ernest\Praca\cake\Cake\Fail\bin\Debug\Fail.dll");
});
JobManager.SetDefault("UnitTests");
