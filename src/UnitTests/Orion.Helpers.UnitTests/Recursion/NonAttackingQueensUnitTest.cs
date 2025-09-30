

using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class NonAttackingQueensUnitTest
    {
        [Fact]
        public void TestCaseSolution1()
        {
            var input = 4;
            var expected = 2;
            var actual_ = new NonAttackingQueensClass();
           var actual = actual_.NonAttachingQueens(input);
                
            Assert.True(expected == actual);
        }
    }
}