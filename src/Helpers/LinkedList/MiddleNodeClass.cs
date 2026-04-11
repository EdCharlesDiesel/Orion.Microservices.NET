namespace Orion.Helpers.LinkedList
{
    public class MiddleNodeClass
    {
        public class LinkedList
        {
            public int Value;
            public LinkedList Next;

            public LinkedList(int value)
            {
                this.Value = value;
                Next = null;
            }
        }

        public LinkedList MiddleNode(LinkedList linkedList)
        {
            LinkedList slowNode = linkedList;
            LinkedList fastnode = linkedList;
            while (fastnode != null && fastnode.Next != null) 
            {
                slowNode = fastnode.Next;
                fastnode = fastnode.Next;
            }

            return slowNode;
        }
    }
}