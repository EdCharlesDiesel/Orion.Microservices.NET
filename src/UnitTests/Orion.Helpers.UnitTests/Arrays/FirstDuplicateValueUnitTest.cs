using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class FirstDuplicateValueClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 2, 1, 5, 2, 3, 3, 4 };
            var expected = 2;
            var actual = new FirstDuplicateValueClass().FirstDuplicateValue(input);
            Assert.True(expected == actual);
        }
    }
}