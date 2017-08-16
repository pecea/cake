// cake using "../../../Files/bin/Debug/Files.dll";

	new Job("CopyDirectory").Does(() => {
		return CopyDirectory(@"D:\Dane\Ernest\Praca\cake\Cake\scripts", @"D:\Dane\Ernest\Copied");
	});

	JobManager.SetDefault("CopyDirectory");