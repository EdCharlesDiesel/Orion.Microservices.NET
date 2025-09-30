using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class MajorityElementClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = new[] { 1, 2, 3, 2, 2, 1, 2 };
            var expected = 2;
            var actual = new MajorityElementClass().MajorityElement(input);
            Assert.True(expected == actual);
        }
    }
}