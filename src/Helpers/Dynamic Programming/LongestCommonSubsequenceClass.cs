namespace Orion.Helpers.Dynamic_Programming
{
    public class LongestCommonSubsequenceClass
    {
        // O(n) time | O(d) space - where n is the number of people
        // in the org and d is the depth (height) of the org chart
        public static OrgChart GetLowestCommonManager(OrgChart topManager, OrgChart reportOne,
        OrgChart reportTwo)
        {
            return GetOrgInfo(topManager, reportOne, reportTwo).LowestCommonManager;
        }
        public static OrgInfo GetOrgInfo(OrgChart manager, OrgChart reportOne, OrgChart reportTwo)
        {
            int numImportantReports = 0;
            foreach (OrgChart directReport in manager.DirectReports)
            {
                OrgInfo orgInfo = GetOrgInfo(directReport, reportOne, reportTwo);
                if (orgInfo.LowestCommonManager != null) return orgInfo;
                numImportantReports += orgInfo.NumImportantReports;
            }
            if (manager == reportOne || manager == reportTwo) numImportantReports++;
            OrgChart lowestCommonManager = numImportantReports == 2 ? manager : null;
            OrgInfo newOrgInfo = new OrgInfo(lowestCommonManager, numImportantReports);
            return newOrgInfo;
        }

        public class OrgChart
        {
            public char Name;
            public List<OrgChart> DirectReports;
            public OrgChart(char name)
            {
                this.Name = name;
                DirectReports = new List<OrgChart>();
            }
            // This method is for testing only.
            public void AddDirectReports(OrgChart[] directReports)
            {
                foreach (OrgChart directReport in directReports)
                {
                    this.DirectReports.Add(directReport);
                }
            }
        }
        public class OrgInfo
        {
            public OrgChart LowestCommonManager;
            public int NumImportantReports;
            public OrgInfo(OrgChart lowestCommonManager, int numImportantReports)
            {
                this.LowestCommonManager = lowestCommonManager;
                this.NumImportantReports = numImportantReports;
            }
        }

        public static List<char> LongestCommonSubsequence(string zxvvyzw, string xkykzpw)
        {
            throw new NotImplementedException();
        }
    }
}