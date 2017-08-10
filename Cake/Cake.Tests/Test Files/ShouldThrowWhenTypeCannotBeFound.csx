new Job("t1").Does(() =>
{
	UnknownType x;
});

JobManager.SetDefault("t1");