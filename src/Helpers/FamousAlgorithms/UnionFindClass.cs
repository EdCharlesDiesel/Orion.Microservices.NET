namespace Orion.Helpers.FamousAlgorithms
{
    /// <summary>
    /// 
    /// </summary>
    public class UnionFindClass
    {
        public class UnionFind
        {
            private Dictionary<int, int> _parents = new Dictionary<int, int>();

            //O(1) time | O(1) space
            public void CreateSet(int value)
            {
                _parents[value] = value;
            }

            public int? Find(int value)
            {
                if (!_parents.ContainsKey(value))
                {
                    return null;
                }

                int currentParent = value;
                while (currentParent != _parents[currentParent])
                {
                    currentParent = _parents[currentParent];
                }
                return currentParent;
            }

            public void Union(int valueOne, int valueTwo)
            {
                if (!_parents.ContainsKey(valueOne) || !_parents.ContainsKey(valueTwo))
                {
                    return;
                }

                int valueOneRoot = (int)Find(valueOne);
                int valueTwoRoot = (int)Find(valueTwo);
                _parents[valueTwoRoot] = valueOneRoot;
            }
        }
    }
}