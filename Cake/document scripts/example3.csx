new VoidJob("FirstJob").DependsOn("ThirdJob", "SecondJob").Does(() =>
{
    Log(LogLevel.Warn,
"I'm the first Job. I depend on the third and the second Job.");
});

new VoidJob("SecondJob").DependsOn("ThirdJob").Does(() =>
{
    Log(LogLevel.Error, "I'm the second Job. I depend on the third Job.");
});

new VoidJob("ThirdJob").Does(() =>
{
    Log(LogLevel.Info, "I'm the third Job.");
});

JobManager.SetDefault("FirstJob");