namespace PassingTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [TestCategory("first")]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("first")]
        [TestCategory("second")]
        public void TestMethod2()
        {
            Assert.AreNotEqual(true, false);
        }

        [TestMethod]
        [TestCategory("second")]
        public void TestMethod3()
        {
            Assert.AreNotEqual(2, 3);
        }
    }
}
