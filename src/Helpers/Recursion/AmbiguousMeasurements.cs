namespace Orion.Helpers.Recursion
{
    /// <summary>
    /// 
    /// </summary>
    public class AmbiguousMeasurementsClass
    {
        public bool AmbiguousMeasurements(int[][] measuringCups, int low, int high)
        {
            Dictionary<string,bool> memoization = new Dictionary<string,bool>();
            return CanMeasureInRange(measuringCups, low, high, memoization);
        }

        private bool CanMeasureInRange(int[][] measuringCups, int low, int high, Dictionary<string, bool> memoization)
        {
            string memorazekey = CreateHashableKey(low, high);
            if (memoization.ContainsKey(memorazekey))
            {
                return memoization[memorazekey];
            }

            if (low <= 0 && high <= 0)
            {
                return false;
            }

            bool canMeasure = false;
            foreach (var cup in measuringCups)
            {
                int cupLow = cup[0];
                int cupHigh = cup[1];

                if (low <= cupLow && cupHigh <= high)
                {
                    canMeasure = true;
                    break;
                }

                int newLow = Math.Max(0,low -cupLow);
                int newHigh = Math.Max(0,high -cupHigh);

                canMeasure = CanMeasureInRange(measuringCups,newLow, newHigh, memoization); 
                if (canMeasure)
                {
                    break;
                }
            }

            memoization[memorazekey]= canMeasure;
            return canMeasure;           
        }

        private string CreateHashableKey(int low, int high)
        {
            return low +":" + high;
        }
    }
}