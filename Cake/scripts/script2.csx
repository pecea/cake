// cake using "../../../Zip/bin/Debug/Zip.dll";
new Job("t1").Does(() => {
	Logger.Log(LogLevel.Trace, "testing");
    return Methods.ZipFiles("NewZipFile", "log.txt", "NLog.dll", ".vs");
});

JobManager.SetDefault("t1");