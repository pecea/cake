#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/NUnit/bin/Debug/NUnit.dll"

new Job("UnitTests").Does(() => {
    return Methods.RunTestsWithOptions(@"D:\Dane\Ernest\Praca\cake\Cake\Success\bin\Debug\Success.dll", "cat == first",
    null, null, null, null, true, true, true, "Info", null, null, "Multiple", "1", null, null, true);
});
JobManager.SetDefault("UnitTests");
