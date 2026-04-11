// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// using Orion.Helpers.LinkedList;
//
// namespace Orion.Helpers.UnitTests.LinkedList
// {
//     public partial class UnitTest1
//     {
//         private GenericClassAlgorithm.LinkedList AddMany(GenericClassAlgorithm.LinkedList ll, List<int> values)
//         {
//             var current = ll;
//             while (current.Next != null)
//             {
//                 current = current.Next;
//             }
//
//             foreach (var value in values)
//             {
//                 current.Next = new GenericClassAlgorithm.LinkedList(value);
//                 current = current.Next;
//             }
//
//             return ll;
//         }
//
//         private List<int> GetNodesInArray(GenericClassAlgorithm.LinkedList ll)
//         {
//             List<int> nodes = new List<int>();
//             var current = ll;
//             while (current != null)
//             {
//                 nodes.Add(current.Value);
//                 current = current.Next;
//             }
//             return nodes;
//         }
//
//         [Fact]
//         public void Test1()
//         {
//             // Arrange
//             var input = new GenericClassAlgorithm.LinkedList(1);
//             AddMany(input, new List<int> { 1, 3, 4, 4, 4, 5, 6, 6 });
//
//             var expectedNodes = new List<int> { 1, 3, 4, 5, 6 };
//
//             // Act
//             var output = new GenericClassAlgorithm.LinkedList(7);
//
//             // Assert
//             Assert.True(Enumerable.SequenceEqual(GetNodesInArray(output), expectedNodes));
//         }
//     }
// }