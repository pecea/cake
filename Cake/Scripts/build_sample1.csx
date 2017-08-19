// cake using "../../../Build/bin/Debug/Build.dll";

new Job("BuildProject").Does(() => BuildProject("../../../Success/Success.csproj"));

JobManager.SetDefault("BuildProject");
