namespace Orion.Helpers.GreedyAlgorithmns
{
    public class OptimalFreelancingClass
    {
        public int OptimalFreelancing(Dictionary<string, int>[] jobs)
        {
            const int lengthOfWeek = 7;
            int profit = 0;
            Array.Sort(jobs, Comparer<Dictionary<string, int>>.Create((jobOne, jobTwo) => jobTwo["payment"]
            .CompareTo(jobOne["payment"])
                    )
            );

            bool[] timeline = new bool[lengthOfWeek];

            foreach( var job in jobs ) 
            {
                int maxTime = Math.Min(job["deadline"], lengthOfWeek);
                for (int time = maxTime-1; time >= 0; time--)
                {
                    if (!timeline[time])
                    {
                        timeline[time] = true;
                        profit += job["payment"];
                        break;
                    }
                }
            }

            return profit;
        }
    }
}