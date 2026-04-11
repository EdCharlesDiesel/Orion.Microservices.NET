using Orion.Helpers.Stacks;

namespace Orion.Helpers.UnitTests.Stacks
{
    public class BalancedBracketsClassUnitTest1 {
        [Fact]
        public void Test1()
        {
            string input = "([])(){}(())()()";
            Assert.True(BalancedBracketsClass.BalancedBrackets(input));
        }
    }
}