//
// namespace Orion.Helpers.UnitTests.LinkedList
// {
//     public class LinkedListUnitTest1
//     {
//         [Fact]
//         public void TestCase1()
//         {
//             TestLinkedList test = new TestLinkedList(0);
//             test.addMany(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
//             int[] expected = { 0, 1, 2, 3, 4, 5, 7, 8, 9 };
//             GenericClassAlgorithm.RemoveKthNodeFromEnd(test, 4);
//             Assert.True(Compare(test.getNodesInArray(), expected));
//         }
//
//         public bool Compare(List<int> arr1, int[] arr2)
//         {
//             if (arr1.Count != arr2.Length)
//             {
//                 return false;
//             }
//
//             for (int i = 0; i < arr1.Count; i++)
//             {
//                 if (arr1[i] != arr2[i])
//                 {
//                     return false;
//                 }
//             }
//
//             return true;
//         }
//     }
// }
//
