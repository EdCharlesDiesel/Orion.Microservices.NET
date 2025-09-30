namespace Orion.Helpers.Stacks
{
    public class MinMaxStackConstructionClass
    {
        List<Dictionary<string, int>> _minMaxStack = new List<Dictionary<string, int>>();
        List<int> _stack = new List<int>();
        // O(1) time | O(1) space
        public int Peek()
        {
            return _stack[_stack.Count - 1];
        }
        // O(1) time | O(1) space
        public int Pop()
        {
            _minMaxStack.RemoveAt(_minMaxStack.Count - 1);
            var val = _stack[_stack.Count - 1];
            _stack.RemoveAt(_stack.Count - 1);
            return val;
        }
        // O(1) time | O(1) space
        public void Push(int number)
        {
            Dictionary<string, int> newMinMax = new Dictionary<string, int>();
            newMinMax.Add("min", number);
            newMinMax.Add("max", number);
            if (_minMaxStack.Count > 0)
            {
                Dictionary<string, int> lastMinMax = new Dictionary<string, int>(
                _minMaxStack[_minMaxStack.Count - 1]
                );
                newMinMax["min"] = Math.Min(lastMinMax["min"], number);
                newMinMax["max"] = Math.Max(lastMinMax["max"], number);
            }
            _minMaxStack.Add(newMinMax);
            _stack.Add(number);
        }
        // O(1) time | O(1) space
        public int GetMin()
        {
            return _minMaxStack[_minMaxStack.Count - 1]["min"];
        }
        // O(1) time | O(1) space
        public int GetMax()
        {
            return _minMaxStack[_minMaxStack.Count - 1]["max"];
        }
    }
}
