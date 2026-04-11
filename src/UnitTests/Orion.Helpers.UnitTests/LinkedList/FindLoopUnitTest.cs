// using Orion.Helpers.LinkedList;
//
// namespace Orion.Helpers.UnitTests.LinkedList
// {
//     public partial class FindLoopClassUnitTest1
//     {
//         [Fact]
//         public void Test1()
//         {
//             TestLinkedList test = new TestLinkedList(0);
//             test.AddMany(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
//             test.GetNthNode(10).next = test.GetNthNode(5);
//             Assert.True(FindLoopClass.FindLoop(test) == test.GetNthNode(5));
//         }
//     }
//
//     public class TestLinkedList : FindLoopClass.LinkedList
//     {
//         public TestLinkedList(int value) : base(value) { }
//
//         public void AddMany(int[] values)
//         {
//             FindLoopClass.LinkedList current = this;
//             while (current.Next != null)
//             {
//                 current = current.Next;
//             }
//             foreach (int value in values)
//             {
//                 current.Next = new FindLoopClass.LinkedList(value);
//                 current = current.Next;
//             }
//         }
//
//         public FindLoopClass.LinkedList GetNthNode(int n)
//         {
//             int counter = 1;
//             FindLoopClass.LinkedList current = this;
//             while (counter < n)
//             {
//                 current = current.Next;
//                 counter++;
//             }
//             return current;
//         }
//     }
// }