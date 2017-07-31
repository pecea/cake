// cake using "../../../Zip/bin/Debug/Zip.dll";
new Job("t1").Does(() => {
    Methods.ZipFiles("NewZipFile", "log.txt");
});

SetDefault("t1");