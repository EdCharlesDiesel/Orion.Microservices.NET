namespace Orion.Helpers.LinkedList
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoveDuplicatesFromLinkedListClass
    {
        public LinkedList RemoveDuplicatesFromLinkedList(LinkedList linkedList) 
        {
            LinkedList currentNode = linkedList;
            while (currentNode != null) 
            {
                LinkedList nextDistince = currentNode.Next;
                while (nextDistince != null && nextDistince.Value == currentNode.Value)
                {
                    nextDistince = nextDistince.Next;
                }

                currentNode.Next = nextDistince;
                currentNode = nextDistince;
            }

            return linkedList;

     
                      
        }
    }


}