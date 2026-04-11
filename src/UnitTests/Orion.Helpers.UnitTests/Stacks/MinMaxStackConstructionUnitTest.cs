namespace Orion.Helpers.UnitTests.Stacks
{
    public class GenericClassAlgorithmUnitTest
    {
        [Fact(Skip="Not implemented yet")]
        public void Test1()
        {
            GenericClassAlgorithm.MinMaxStack stack = new GenericClassAlgorithm.MinMaxStack();
            stack.Push(5);
            TestMinMaxPeek(5, 5, 5, stack);
            stack.Push(7);
            TestMinMaxPeek(5, 7, 7, stack);
            stack.Push(2);
            TestMinMaxPeek(2, 7, 2, stack);
            Assert.True(stack.Pop() == 2);
            Assert.True(stack.Pop() == 7);
            TestMinMaxPeek(5, 5, 5, stack);
        }

        public void TestMinMaxPeek(int min, int max, int Peek, GenericClassAlgorithm.MinMaxStack stack)
        {
            Assert.True(stack.GetMin() == min);
            Assert.True(stack.GetMax() == max);
            Assert.True(stack.Peek() == Peek);
        }
    }
}