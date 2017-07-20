namespace Build.Tests.TestProject
{
    public class Class1
    {
    }

#if DEBUG
    public class Debug
    {
    }
#else
    public class Release
    {
    }
#endif
}
