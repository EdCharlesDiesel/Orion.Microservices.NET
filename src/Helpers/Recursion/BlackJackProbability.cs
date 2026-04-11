namespace Orion.Helpers.Recursion
{
    public class BlackJackProbability
    {
        public double BlackjackProbability(int target, int startingHand)
        {
            Dictionary<int,double> memo = new Dictionary<int,double>();
            return Math.Round(CalculateProbability(target, startingHand,memo)* 1000f)/1000f;  
        }

        private double CalculateProbability(int target, int currentHand, Dictionary<int, double> memo)
        {
            if (memo.ContainsKey(currentHand))
            {
                return memo[currentHand];
            }

            if(currentHand> target){
                return 1;
            }

            if (currentHand + 4 >= target)
            {
                return 0;
            }
            double totalProbelity = 0;
            for (int drawCard = 1; drawCard <= 10; drawCard++)
            {
                totalProbelity += .1 * CalculateProbability(target, currentHand + drawCard, memo);
            }
            memo[currentHand] = totalProbelity;
            return totalProbelity;
        }
    }
}