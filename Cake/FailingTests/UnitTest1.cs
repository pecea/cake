using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FailingTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [TestCategory("first")]
        public void TestMethod1()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [TestCategory("first")]
        [TestCategory("second")]
        public void TestMethod2()
        {
            Assert.AreNotEqual(false, false);
        }

        [TestMethod]
        [TestCategory("second")]
        public void TestMethod3()
        {
            Assert.AreNotEqual(2, 2);
        }
    }
}
