

using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class NonConstructibleChangeClassTest
    {   

        [Fact]
        public void Test1()
        {
            int[] input = new[] { 5, 7, 1, 1, 2, 3, 22 };
            int expected = 20;
            var actual = new NonConstructibleChangeClass().NonConstructibleChange(input);
            Assert.True(expected == actual);
        }
    }
}