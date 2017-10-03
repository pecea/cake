var job1 = new Job("J1").Does(() => true).DependsOn("J2");
var job2 = new Job("J2").Does(() => true).DependsOn("J3");
var job3 = new Job("J3").Does(() => true).DependsOn(job1);

JobManager.SetDefault("J3");