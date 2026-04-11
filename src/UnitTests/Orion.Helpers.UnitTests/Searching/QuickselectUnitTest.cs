using Orion.Helpers.Searching;

namespace Orion.Helpers.UnitTests.Searching
{
    public class QuickselectClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(
              QuickselectClass.Quickselect(new[] { 8, 5, 2, 9, 7, 6, 3 }, 3) == 5
            );
        }
    }
}