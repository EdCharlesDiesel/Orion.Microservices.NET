namespace Orion.Helpers.Heaps
{
    public class MinHeapConstructionClass
    {
        public List<int> Heap = new List<int>();
        public MinHeapConstructionClass(List<int> array)
        {
            Heap = BuildHeap(array);
        }
        // O(n) time | O(1) space
        public List<int> BuildHeap(List<int> array)
        {
            int firstParentIdx = (array.Count - 2) / 2;
            for (int currentIdx = firstParentIdx; currentIdx >= 0; currentIdx--)
            {
                SiftDown(currentIdx, array.Count - 1, array);
            }
            return array;
        }
        // O(log(n)) time | O(1) space
        public void SiftDown(int currentIdx, int endIdx, List<int> heap)
        {
            int childOneIdx = currentIdx * 2 + 1;
            while (childOneIdx <= endIdx)
            {
                int childTwoIdx = currentIdx * 2 + 2 <=
                endIdx ? currentIdx * 2 + 2 : -1;
                int idxToSwap;
                if (childTwoIdx != -1 && heap[childTwoIdx] < heap[childOneIdx])
                {
                    idxToSwap = childTwoIdx;
                }
                else
                {
                    idxToSwap = childOneIdx;
                }
                if (heap[idxToSwap] < heap[currentIdx])
                {
                    Swap(currentIdx, idxToSwap, heap);
                    currentIdx = idxToSwap;
                    childOneIdx = currentIdx * 2 + 1;
                }
                else
                {
                    return;
                }
            }
        }

        // O(log(n)) time | O(1) space
        public void SiftUp(int currentIdx, List<int> heap)
        {
            int parentIdx = (currentIdx - 1) / 2;
            while (currentIdx > 0 && heap[currentIdx] < heap[parentIdx])
            {
                Swap(currentIdx, parentIdx, heap);
                currentIdx = parentIdx;
                parentIdx = (currentIdx - 1) / 2;
            }
        }
        public int Peek()
        {
            return Heap[0];
        }
        public int Remove()
        {
            Swap(0, Heap.Count - 1, Heap);
            int valueToRemove = Heap[Heap.Count - 1];
            Heap.RemoveAt(Heap.Count - 1);
            SiftDown(0, Heap.Count - 1, Heap);
            return valueToRemove;
        }
        public void Insert(int value)
        {
            Heap.Add(value);
            SiftUp(Heap.Count - 1, Heap);
        }
        public void Swap(int i, int j, List<int> heap)
        {
            int temp = heap[j];
            heap[j] = heap[i];
            heap[i] = temp;
        }
    }
}