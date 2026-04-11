using Orion.Helpers.LinkedList;

namespace Orion.Helpers.UnitTests.LinkedList
{
    public partial class MiddleNodeClassUnitTest1
    {
        [Fact(Skip = "Not Implemented yet")]
        public void Test1()
        {
            var linkedList = new MiddleNodeClass.LinkedList(1);
            var curr = linkedList;
            for (int i = 1; i < 4; i++)
            {
                curr.Next = new MiddleNodeClass.LinkedList(i);
                curr = curr.Next;
            }

            List<int> expected = new List<int> {2, 3};
            var actual = new MiddleNodeClass().MiddleNode(linkedList);
            Assert.True(expected.SequenceEqual(ToList(actual)));
        }

        private List<int> ToList(MiddleNodeClass.LinkedList linkedList)
        {
            List<int> list = new List<int>();
            MiddleNodeClass.LinkedList curr = linkedList;
            while (curr != null)
            {
                list.Add(curr.Value);
                curr = curr.Next;
            }

            return list;
        }
    }
}