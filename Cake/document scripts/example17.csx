#r "C:/Users/ErnestPrzestrzelski/Desktop/Praca/Cake/Git/bin/Debug/Git.dll"

 using System;

 new VoidJob("GitInit").Does(() =>
 {
     Methods.RepositoryPath = @"C:\Repository";
     Methods.UserIdentity = Identity.FromJsonFile(@"C:\creds.json");
 });

 new Job("Commit").DependsOn("GitInit").Does(() =>
 {
     string input;

     Logger.Log(LogLevel.Info, "These changes will be committed:");
     Methods.DiffWorkingDir();

     Logger.Log(LogLevel.Warn, "Commit these changes? (y/n)");

     do input = Console.ReadLine();
     while (input?.ToLower() != "y" && input?.ToLower() != "n");

     return input.ToLower() == "y" && Methods.CommitAllChanges();
 });

 new Job("Pull").DependsOn("Commit").Does(Methods.Pull);

 new Job("Push").DependsOn("Pull").Does(Methods.Push);

 JobManager.SetDefault("Push");
