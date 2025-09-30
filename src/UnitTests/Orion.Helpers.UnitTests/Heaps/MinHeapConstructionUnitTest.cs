// using System.Collections.Generic;
// using Xunit;
// using Orion.Helpers;
//
// namespace Orion.Helpers.UnitTests.Heaps
// {
//     public class MinHeapUnitTest
//     {
//         [Fact]
//         public void Test1()
//         {
//             // Arrange
//             var minHeap = new GenericClassAlgorithm.MinHeap(
//                 new List<int> { 48, 12, 24, 7, 8, -5, 24, 391, 24, 56, 2, 6, 8, 41 }
//             );
//
//             // Act & Assert
//             minHeap.Insert(76);
//             Assert.True(IsMinHeapPropertySatisfied(minHeap.heap));
//             Assert.Equal(-5, minHeap.Peek());
//
//             Assert.Equal(-5, minHeap.Remove());
//             Assert.True(IsMinHeapPropertySatisfied(minHeap.heap));
//             Assert.Equal(2, minHeap.Peek());
//
//             Assert.Equal(2, minHeap.Remove());
//             Assert.True(IsMinHeapPropertySatisfied(minHeap.heap));
//             Assert.Equal(6, minHeap.Peek());
//
//             minHeap.Insert(87);
//             Assert.True(IsMinHeapPropertySatisfied(minHeap.heap));
//         }
//
//         private bool IsMinHeapPropertySatisfied(List<int> array)
//         {
//             for (int currentIdx = 1; currentIdx < array.Count; currentIdx++)
//             {
//                 int parentIdx = (currentIdx - 1) / 2;
//                 if (parentIdx >= 0 && array[parentIdx] > array[currentIdx])
//                 {
//                     return false;
//                 }
//             }
//             return true;
//         }
//     }
// }