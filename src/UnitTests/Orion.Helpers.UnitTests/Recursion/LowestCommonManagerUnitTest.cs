


using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class LowestCommonManagerUnitTest
    {
        public Dictionary<char, LowestCommonManagerClass.OrgChart> getOrgCharts()
        {
            var orgCharts = new Dictionary<char, LowestCommonManagerClass.OrgChart>();
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            foreach (char a in alphabet)
            {
                orgCharts.Add(a, new LowestCommonManagerClass.OrgChart(a));
            }
            return orgCharts;
        }

        [Fact]
        public void TestCase1()
        {
            var orgCharts = getOrgCharts();
            orgCharts['A'].AddDirectReports(new[] {orgCharts['B'],
                                                                orgCharts['C']});
            orgCharts['B'].AddDirectReports(new[] {orgCharts['D'],
                                                                orgCharts['E']});
            orgCharts['C'].AddDirectReports(new[] {orgCharts['F'],
                                                                orgCharts['G']});
            orgCharts['D'].AddDirectReports(new[] {orgCharts['H'],
                                                                orgCharts['I']});
            LowestCommonManagerClass.OrgChart lcm = LowestCommonManagerClass.GetLowestCommonManager(orgCharts['A'],
                orgCharts['E'],
                orgCharts['I']);
            Assert.True(lcm == orgCharts['B']);
        }
    }
}