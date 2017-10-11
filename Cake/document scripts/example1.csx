new VoidJob("FirstJob").Does(() =>
{
    System.Console.WriteLine("I am the first job.");
});

JobManager.SetDefault("FirstJob"); 