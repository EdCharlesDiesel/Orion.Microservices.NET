// namespace Orion.Helpers.UnitTests.LinkedList
// {
//     public partial class UnitTest1
//     {
//         [Fact]
//         public void Test1()
//         {
//             TestLinkedList list1 = new TestLinkedList(2);
//             list1.addMany(new List<int>() { 6, 7, 8 });
//             TestLinkedList list2 = new TestLinkedList(1);
//             list2.addMany(new List<int>() { 3, 4, 5, 9, 10 });
//             TestLinkedList output =
//               (TestLinkedList)Program.mergeLinkedLists(list1, list2);
//             List<int> expectedNodes = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//             Assert.True(output.getNodesInArray().SequenceEqual(expectedNodes));
//         }
//     }
//
//     public class TestLinkedList : MergeLinkedListsClass.LinkedList
//     {
//         public TestLinkedList(int val) : base(val) { }
//
//         public TestLinkedList AddMany(List<int> values)
//         {
//             TestLinkedList current = this;
//             while (current.Next != null)
//             {
//                 current = (TestLinkedList)current.Next;
//             }
//             foreach (int value in values)
//             {
//                 current.Next = new TestLinkedList(value);
//                 current = (TestLinkedList)current.Next;
//             }
//             return this;
//         }
//
//         public List<int> GetNodesInArray()
//         {
//             List<int> nodes = new List<int>();
//             TestLinkedList current = this;
//             while (current != null)
//             {
//                 nodes.Add(current.Value);
//                 current = (TestLinkedList)current.Next;
//             }
//             return nodes;
//         }
//     }
// }