namespace Orion.Helpers.Heaps
{
    public class ContinuousMedianClass
    {
        public List<int> Heap = new List<int>();
        public Func<int, int, bool> ComparisonFunc;
        public int Length;
        public ContinuousMedianClass(Func<int, int, bool> func, List<int> array)
        {
            Heap = BuildHeap(array);
            ComparisonFunc = func;
            Length = Heap.Count;
        }
        public int Peek()
        {
            return Heap[0];
        }
        public int Remove()
        {
            Swap(0, Heap.Count - 1);
            int valueToRemove = Heap[Heap.Count - 1];
            Heap.RemoveAt(Heap.Count - 1);
            Length -= 1;
            SiftDown(0, Heap.Count - 1, Heap);
            return valueToRemove;
        }
        public void Insert(int value)
        {
            Heap.Add(value);
            Length += 1;
            SiftUp(Heap.Count - 1, Heap);
        }
        public List<int> BuildHeap(List<int> array)
        {
            int firstParentIdx = (array.Count - 2) / 2;
            for (int currentIdx = firstParentIdx; currentIdx >= 0; currentIdx--)
            {
                SiftDown(currentIdx, array.Count - 1, array);
            }
            return array;
        }
        public void SiftDown(int currentIdx, int endIdx, List<int> heap)
        {
            int childOneIdx = currentIdx * 2 + 1;
            while (childOneIdx <= endIdx)
            {
                int childTwoIdx = currentIdx * 2 + 2 <=
                endIdx ? currentIdx * 2 + 2 : -1;
                int idxToSwap;
                if (childTwoIdx != -1)
                {
                    if (ComparisonFunc(heap[childTwoIdx], heap[childOneIdx]))
                    {
                        idxToSwap = childTwoIdx;
                    }
                    else
                    {
                        idxToSwap = childOneIdx;
                    }
                }
                else
                {
                    idxToSwap = childOneIdx;
                }
                if (ComparisonFunc(heap[idxToSwap], heap[currentIdx]))
                {
                    Swap(currentIdx, idxToSwap);
                    currentIdx = idxToSwap;
                    childOneIdx = currentIdx * 2 + 1;
                }
                else
                {
                    return;
                }
            }
        }
        public void SiftUp(int currentIdx, List<int> heap)
        {
            int parentIdx = (currentIdx - 1) / 2;
            while (currentIdx > 0)
            {
                if (ComparisonFunc(heap[currentIdx], heap[parentIdx]))
                {
                    Swap(currentIdx, parentIdx);
                    currentIdx = parentIdx;
                    parentIdx = (currentIdx - 1) / 2;
                }
                else
                {
                    return;
                }
            }
        }
        public void Swap(int i, int j)
        {
            int temp = Heap[j];
            Heap[j] = Heap[i];
            Heap[i] = temp;
        }
    }
}