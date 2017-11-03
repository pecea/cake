using System;

namespace TestLib
{
    public class Class1
    {
        public void Elo()
        {
#if DEBUG
            Console.WriteLine("DEBUG");
#else
            Console.WriteLine("RELEASE");
#endif
            Console.ReadKey();
        }
    }
}
