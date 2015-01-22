using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
