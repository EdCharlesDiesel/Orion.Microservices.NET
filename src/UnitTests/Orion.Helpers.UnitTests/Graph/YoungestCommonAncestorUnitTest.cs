//
//
// using Orion.Helpers.Graphs;
//
// namespace Orion.Helpers.UnitTests.Graph
// {
//     public class AncestralTreeUnitTest
//     {
//         [Fact]
//         public void Test1()
//         {
//             var trees = GetNewTrees();
//             trees['A'].AddAsAncestor(
//               new YoungestCommonAncestorClass.AncestralTree[] { trees['B'], trees['C'] }
//             );
//             trees['B'].AddAsAncestor(
//               new YoungestCommonAncestorClass.AncestralTree[] { trees['D'], trees['E'] }
//             );
//             trees['D'].AddAsAncestor(
//               new YoungestCommonAncestorClass.AncestralTree[] { trees['H'], trees['I'] }
//             );
//             trees['C'].AddAsAncestor(
//               new YoungestCommonAncestorClass.AncestralTree[] { trees['F'], trees['G'] }
//             );
//
//             YoungestCommonAncestorClass.AncestralTree yca =
//                 YoungestCommonAncestorClass.GetYoungestCommonAncestor(trees['A'], trees['E'], trees['I']);
//             Assert.True(yca == trees['B']);
//         }
//
//         private object GetNewTrees()
//         {
//             throw new NotImplementedException();
//         }
//     }
//
//     public Dictionary<char, GenericClassAlgorithm.AncestralTree> GetNewTrees()
//     {
//         var trees = new Dictionary<char, GenericClassAlgorithm.AncestralTree>();
//         var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
//         foreach (char a in alphabet)
//         {
//             trees.Add(a, new GenericClassAlgorithm.AncestralTree(a));
//         }
//
//         trees['A'].AddAsAncestor(new[] {
//       trees['B'], trees['C'], trees['D'], trees['E'], trees['F']
//     });
//         return trees;
//     }
// }