

namespace Orion.Helpers.UnitTests.LinkedList
{
    public partial class GenericClassAlgorithmUnitTest1
    {
        [Fact]
        public void Test1()
        {
            void TestCase1()
            {
                GenericClassAlgorithm.LinkedList test = newLinkedList(new int[] { 0, 1, 2, 3, 4, 5 });
                List<int> result = toList(GenericClassAlgorithm.ReverseLinkedList(test));
                int[] expected = new int[] { 5, 4, 3, 2, 1, 0 };
                Assert.True(arraysEqual(result, expected));
            }

            GenericClassAlgorithm.LinkedList newLinkedList(int[] values)
            {
                GenericClassAlgorithm.LinkedList ll = new GenericClassAlgorithm.LinkedList(values[0]);
                GenericClassAlgorithm.LinkedList current = ll;
                for (int i = 1; i < values.Length; i++)
                {
                    current.Next = new GenericClassAlgorithm.LinkedList(values[i]);
                    current = current.Next;
                }
                return ll;
            }

            List<int> toList(GenericClassAlgorithm.LinkedList ll)
            {
                List<int> arr = new List<int>();
                GenericClassAlgorithm.LinkedList current = ll;
                while (current != null)
                {
                    arr.Add(current.Value);
                    current = current.Next;
                }
                return arr;
            }

            bool arraysEqual(List<int> arr1, int[] arr2)
            {
                if (arr1.Count != arr2.Length) return false;
                for (int i = 0; i < arr1.Count; i++)
                {
                    if (arr1[i] != arr2[i]) return false;
                }
                return true;
            }
        }
    }
    
}