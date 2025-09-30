namespace Orion.Helpers.Recursion
{
    /// <summary>
    /// You're given three inputs, all of which are instances of an OrgChart class that have a 
    /// directReports property pointing to their direct reports. The first input is the top manager
    /// in an organizational chart(i.e., the only instance that isn't anybody else's direct report),
    /// and the other two inouts are reports in the organization chart. The two reports are guatanteed
    /// to be district.
    /// 
    /// Write function that returns the lowest common manager to the two reports.
    /// </summary>
    public static  class LowestCommonManagerClass
    {
        //O(n) time | O(d) space - where n is the number
        public static OrgChart GetLowestCommonManager(OrgChart topManager,OrgChart reportOne,
            OrgChart reportTwo)
        {
            return GetOrgInfo(topManager, reportOne, reportTwo).LowerCommonManager;
        }

        public static OrgInfo GetOrgInfo(OrgChart manager, OrgChart reportOne, OrgChart reportTwo)
        {
            int numImportantReports = 0;
            foreach (OrgChart directReport in manager.DirectReports)
            {
                OrgInfo orgInfo = GetOrgInfo(directReport, reportOne, reportTwo);
                if (orgInfo.LowerCommonManager != null) return orgInfo;

                numImportantReports += orgInfo.NumImportantReports;
            }

            if (manager == reportOne || manager == reportTwo) {
             numImportantReports++;
            }

            OrgChart lowerCommonManager = numImportantReports == 2 ? manager : null;
            OrgInfo newOrgInfo = new OrgInfo(lowerCommonManager, numImportantReports);


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
            public OrgChart LowerCommonManager;
            public int NumImportantReports;

            public OrgInfo(OrgChart lowerCommonManager, int numImportReport)
            {
                this.LowerCommonManager = lowerCommonManager;
                NumImportantReports = numImportReport;
            }
        }
    }    

    
}