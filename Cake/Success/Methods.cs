using NUnit.Framework;

namespace Success
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [TestCase(Category = "first")]
        [Property("Priority", "High")]
        public void Test1()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        [TestCase(Category = "second")]
        public void Test2()
        {
            Assert.AreEqual(2, 2);
        }
    }
}
