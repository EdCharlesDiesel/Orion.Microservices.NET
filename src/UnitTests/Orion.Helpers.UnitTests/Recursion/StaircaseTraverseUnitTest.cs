

using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class StaircaseTraverseUnitTest
    {
        [Fact]
        public void Test1()
        {
            int stairs = 4;
            int maxSteps = 2;
            int expected = 5;
            int actual = StaircaseTraverse.StaircaseTraversal(stairs, maxSteps);
            Assert.True(expected == actual);
        }
    }
}