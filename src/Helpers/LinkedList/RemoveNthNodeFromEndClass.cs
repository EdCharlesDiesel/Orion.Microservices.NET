﻿namespace Orion.Helpers.LinkedList
{
    public class RemoveNthNodeFromEndClass
    {
        // O(n) time | O(1) space
        public static void RemoveKthNodeFromEnd(LinkedList head, int k)
        {
            int counter = 1;
            LinkedList first = head;
            LinkedList second = head;
            while (counter <= k)
            {
                second = second.Next;
                counter++;
            }
            if (second == null)
            {
                head.Value = head.Next.Value;
                head.Next = head.Next.Next;
                return;
            }
            while (second.Next != null)
            {
                second = second.Next;
                first = first.Next;
            }
            first.Next = first.Next.Next;
        }
        public class LinkedList
        {
            public int Value;
            public LinkedList Next;
            public LinkedList(int value)
            {
                Value = value;
            }
        }
    }
}