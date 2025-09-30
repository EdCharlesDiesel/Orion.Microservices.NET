namespace Orion.Helpers.Graphs
{
    public abstract class LongestIncreasingSubsequenceClass
    {
        // O(n^2) time | O(n) space
        public static List<int> LongestIncreasingSubsequence(int[] array)
        {
            var sequences = new int[array.Length];
            Array.Fill(sequences, int.MinValue);
            var lengths = new int[array.Length];
            Array.Fill(lengths, 1);
            var maxLengthIdx = 0;
            for (var i = 0; i < array.Length; i++)
            {
                var currentNum = array[i];
                for (var j = 0; j < i; j++)
                {
                    var otherNum = array[j];
                    if (otherNum >= currentNum || lengths[j] + 1 < lengths[i]) continue;
                    lengths[i] = lengths[j] + 1;
                    sequences[i] = j;
                }
                if (lengths[i] >= lengths[maxLengthIdx])
                {
                    maxLengthIdx = i;
                }
            }
            return BuildSequence(array, sequences, maxLengthIdx);
        }

        private static List<int> BuildSequence(int[] array, int[] sequences, int currentIdx)
        {
            var sequence = new List<int>();
            while (currentIdx != int.MinValue)
            {
                sequence.Insert(0, array[currentIdx]);
                currentIdx = sequences[currentIdx];
            }
            return sequence;
        }
    }
}