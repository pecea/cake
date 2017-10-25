using System;

var lifeSaver = new Job("YEAH")
    .Does(() =>
    {
        Logger.Log(LogLevel.Trace, $"Cake started for script: OIFSHOISFHOISFA.");

        new SomeClass().ThrowShit();
        return true;
    })
    .OnException("SEEMS_SAFE");
    //.DependsOn("SEEMS_SAFE");

var throwingLeafJob = new Job("LEAF_THROW")
    .Does(() =>
    {
        return null;
        throw new NotImplementedException("OOPSIE LEAF_THROW");
    })
    .DependsOn(lifeSaver)
    .OnException(lifeSaver);

var almostSafeJob = new VoidJob("SEEMS_SAFE")
    .Does(() =>
    {
        return;
        throw new NotImplementedException("OOPSIE SEEMS_SAFE");
    })
    .DependsOn(throwingLeafJob)
    .OnException(throwingLeafJob);

var mainJob = new Job("MAIN")
    .Does(() => true)
    .DependsOn(almostSafeJob);

JobManager.SetDefault(mainJob);

public class SomeClass
{
    public void ThrowShit()
    {
        throw new InvalidCastException("LOL");
    }
}