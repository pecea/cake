// cake using "../../../MsTest/bin/Debug/MsTest.dll";
new Job("t1").Does(() => {
    Methods.RunTests("../../Test Files/PassingTests.dll", null, null, null, null);
});

SetDefault("t1");