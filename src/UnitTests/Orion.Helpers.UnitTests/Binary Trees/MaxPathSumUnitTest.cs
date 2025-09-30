// namespace Orion.Helpers.UnitTests.Binary_Trees
// {
//     public class MaxPathSumUnitTest
//     {
//         [Fact]
//         public void Test1()
//         {
//             var test = new TestBinaryTree(1);
//             test.Insert(new[] { 2, 3, 4, 5, 6, 7 }, 0);
//
//             var _ = GenericClassAlgorithm.MaxPathSum(test) == 18;
//             Assert.True(_);
//         }
//
//         public class TestBinaryTree : GenericClassAlgorithm.BinaryTree
//         {
//             public TestBinaryTree(int value) : base(value) { }
//
//             public void Insert(int[] values, int i)
//             {
//                 if (i >= values.Length)
//                 {
//                     return;
//                 }
//                 List<GenericClassAlgorithm.BinaryTree> queue = new List<GenericClassAlgorithm.BinaryTree>();
//                 queue.Add(this);
//                 var index = 0;
//                 while (index < queue.Count)
//                 {
//                     GenericClassAlgorithm.BinaryTree current = queue[index];
//                     index += 1;
//                     if (current.Left == null)
//                     {
//                         current.Left = new GenericClassAlgorithm.BinaryTree(values[i]);
//                         break;
//                     }
//                     queue.Add(current.Left);
//                     if (current.Right == null)
//                     {
//                         current.Right = new GenericClassAlgorithm.BinaryTree(values[i]);
//                         break;
//                     }
//                     queue.Add(current.Right);
//                 }
//                 Insert(values, i + 1);
//             }
//         }
//     }
// }