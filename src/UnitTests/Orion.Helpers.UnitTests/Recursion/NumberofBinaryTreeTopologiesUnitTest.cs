using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class NumberofBinaryTreeTopologiesTests
    { 
        [Fact]
        public void TestCaseSolutionOne()
        {
            var numberofBinaryTreeTopologiesClass = new NumberofBinaryTreeTopologiesClass();
       
            Assert.True(numberofBinaryTreeTopologiesClass.NumberofBinaryTreeTopologies(3) == 5);
        }
    }
}