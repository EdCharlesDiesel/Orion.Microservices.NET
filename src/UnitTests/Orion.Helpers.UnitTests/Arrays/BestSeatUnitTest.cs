using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class BestSeatClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 1, 0, 1, 0, 0, 0, 1 };
            var expected = 4;
            var actual = new BestSeatClass().BestSeat(input);
            Assert.True(expected == actual);
        }
    }
}