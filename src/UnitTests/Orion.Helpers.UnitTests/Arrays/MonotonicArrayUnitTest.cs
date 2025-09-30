using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class IsMonotonicClassTest
    {    

        [Fact]
        public void Test1()
        {
            var array = new[] { -1, -5, -10, -1100, -1100, -1101, -1102, -9001 };
            var actual = IsMonotonicClass.IsMonotonic(array);
           Assert.True(true);
        }
    }
}