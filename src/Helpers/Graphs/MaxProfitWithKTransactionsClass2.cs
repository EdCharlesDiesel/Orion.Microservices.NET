﻿namespace Orion.Helpers.Graphs
{
    internal class MaxProfitWithKTransactionsClass2
    {
        // O(nk) time | O(n) space
        public static int MaxProfitWithKTransactions(int[] prices, int k)
        {
            if (prices.Length == 0)
            {
                return 0;
            }
            var evenProfits = new int[prices.Length];
            var oddProfits = new int[prices.Length];
            for (var i = 0; i < prices.Length; i++)
            {
                evenProfits[i] = 0;
                oddProfits[i] = 0;
            }
            for (int t = 1; t < k + 1; t++)
            {
                var maxThusFar = int.MinValue;
                int[] currentProfits;
                int[] previousProfits;
                if (t % 2 == 1)
                {
                    currentProfits = oddProfits;
                    previousProfits = evenProfits;
                }
                else
                {
                    currentProfits = evenProfits;
                    previousProfits = oddProfits;
                }
                for (var d = 1; d < prices.Length; d++)
                {
                    maxThusFar =
                    Math.Max(maxThusFar,
                    previousProfits[d - 1] - prices[d - 1]);
                    currentProfits[d] = Math.Max(currentProfits[d - 1],
                    maxThusFar + prices[d]);
                }
            }
            return k % 2 == 0 ? evenProfits[prices.Length - 1] : oddProfits[prices.Length - 1];
        }
    }
}
