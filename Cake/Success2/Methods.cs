using Xunit;

namespace Success2
{
    public class Methods
    {
        [Fact]
        [Trait("Category", "first")]
        public void Test1()
        {
            Assert.True(true);
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal(2, 1+1);
        }
    }
}
