using Orion.Helpers.Searching;

namespace Orion.Helpers.UnitTests.Searching
{
    public class ShiftedBinarySearchClassUnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(
                 ShiftedBinarySearchClass.ShiftedBinarySearch(
                   new[] { 45, 61, 71, 72, 73, 0, 1, 21, 33, 37 }, 33
                 ) == 8
               );
        }
    }
}