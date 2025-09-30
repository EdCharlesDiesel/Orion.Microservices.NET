
namespace Orion.Helpers.UnitTests.LinkedList
{
    public partial class GenericClassAlgorithmUnitTest
    {
        public List<int> LinkedListToArray(GenericClassAlgorithm.LinkedList head)
        {
            var array = new List<int>();
            var current = head;
            while (current != null)
            {
                array.Add(current.Value);
                current = current.Next;
            }
            return array;
        }
        [Fact]
        public void Test1()
        {
            var head = new GenericClassAlgorithm.LinkedList(0);
            head.Next = new GenericClassAlgorithm.LinkedList(1);
            head.Next.Next = new GenericClassAlgorithm.LinkedList(2);
            head.Next.Next.Next = new GenericClassAlgorithm.LinkedList(3);
            head.Next.Next.Next.Next = new GenericClassAlgorithm.LinkedList(4);
            head.Next.Next.Next.Next.Next = new GenericClassAlgorithm.LinkedList(5);
            var result = GenericClassAlgorithm.ShiftLinkedList(head, 2);
            var array = this.LinkedListToArray(result);

            var expected = new List<int> { 4, 5, 0, 1, 2, 3 };
            Assert.True(Enumerable.SequenceEqual(expected, array));
        }
    }
}