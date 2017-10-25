#r "C:\Users\Piotr\Source\Repos\cake\Cake\Git\bin\Debug\Git.dll"

using System;

new VoidJob("GitInit").Does(() =>
{
    RepositoryPath = @"C:\Repository";
    UserIdentity = Identity.FromJsonFile(@"C:\creds.json");
});

new Job("Commit").DependsOn("GitInit").Does(() =>
{
    string input;

    Logger.Log(LogLevel.Info, "These changes will be committed:");
    DiffWorkingDir();

    Logger.Log(LogLevel.Warn, "Commit these changes? (y/n)");

    do input = Console.ReadLine();
    while (input.ToLower() != "y" && input.ToLower() != "n");

    return input.ToLower() == "y"
        ? CommitAllChanges()
        : false;
});

new Job("Pull").DependsOn("Commit").Does(Pull);

new Job("Push").DependsOn("Pull").Does(Push);

JobManager.SetDefault("Push");

