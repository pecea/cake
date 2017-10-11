using System;

new VoidJob("Test").Does(() =>
{
    Console.WriteLine("Testing stuff...");
});
new VoidJob("Deploy").DependsOn("Test").Does(() =>
{
    Console.WriteLine("Heavy deploy action");
});

JobManager.SetDefault("Deploy");