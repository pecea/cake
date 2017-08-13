using NUnit.Framework;

namespace Fail
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual(1, 2);
        }
    }
}
