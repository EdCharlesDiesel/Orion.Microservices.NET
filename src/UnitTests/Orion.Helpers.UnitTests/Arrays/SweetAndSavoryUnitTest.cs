using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class SweetAndSavoryClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            int[] dishes = new[] { -3, -5, 1, 7 };
            int target = 8;
            int[] expected = new[] { -3, 7 };
            int[] actual = new SweetAndSavoryClass().SweetAndSavory(dishes, target);
            Assert.True(actual.Length == 2);
            Assert.True(actual[0] == expected[0]);
            Assert.True(actual[1] == expected[1]);
        }
    }
}