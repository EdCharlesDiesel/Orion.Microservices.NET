using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class IndexEqualsValueClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(
              new IndexEqualsValueClass().IndexEqualsValue(new[] { -5, -3, 0, 3, 4, 5, 9 }) == 3
            );

        }
    }
}