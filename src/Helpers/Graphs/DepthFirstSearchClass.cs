namespace Orion.Helpers.Graphs
{
    public class DepthFirstSearchClass
    {
        public class Node
        {
            public string Name;
            public List<Node> Children = new List<Node>();

            public Node(string name)
            {
                this.Name = name;
            }

            public List<string> DepthFirstSearch(List<string> array)
            {
                array.Add(Name);
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].DepthFirstSearch(array);
                }
                return array;
            }

            public Node AddChild(string name)
            {
                Node child = new Node(name);
                Children.Add(child);
                return this;
            }
        }
    }
}