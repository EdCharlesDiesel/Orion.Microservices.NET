namespace Orion.Helpers.Tries
{
    public class SuffixTrieConstructionClass
    {
        public class TrieNode
        {
            public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
        }
        public class SuffixTrie
        {
            public TrieNode Root = new TrieNode();
            public char EndSymbol = '*';
            public SuffixTrie(string str)
            {
                PopulateSuffixTrieFrom(str);
            }
            // O(n^2) time | O(n^2) space
            public void PopulateSuffixTrieFrom(string str)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    InsertSubstringStartingAt(i, str);
                }
            }
            public void InsertSubstringStartingAt(int i, string str)
            {
                TrieNode node = Root;
                for (int j = i; j < str.Length; j++)
                {
                    char letter = str[j];
                    if (!node.Children.ContainsKey(letter))
                    {
                        TrieNode newNode = new TrieNode();
                        node.Children.Add(letter, newNode);
                    }
                    node = node.Children[letter];
                }
                node.Children[EndSymbol] = null;
            }
            // O(m) time | O(1) space
            public bool Contains(string str)
            {
                TrieNode node = Root;
                for (int i = 0; i < str.Length; i++)
                {
                    char letter = str[i];
                    if (!node.Children.ContainsKey(letter))
                    {
                        return false;
                    }
                    node = node.Children[letter];
                }
                return node.Children.ContainsKey(EndSymbol);
            }
        }
    }
}