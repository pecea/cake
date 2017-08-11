// cake using "../../../NUnit/bin/Debug/NUnit.dll";

new Job("t3").Does(() => {
    Methods.RunTests("test");
});
JobManager.SetDefault("t3");