using Xunit;

namespace Fail2
{
    public class Methods
    {
        [Fact]
        [Trait("Category", "second")]
        public void Test1()
        {
            Assert.True(false);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(2, 1);
        }
    }
}
