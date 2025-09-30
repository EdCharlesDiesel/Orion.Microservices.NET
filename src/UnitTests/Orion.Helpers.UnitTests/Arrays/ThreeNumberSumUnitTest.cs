using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays;

public class ThreeNumberSumClassUnitTest
{
    [Fact]
    public void Test1()
    {


        List<int[]> expected = new List<int[]>();
        expected.Add(new[] { -8, 2, 6 });
        expected.Add(new[] { -8, 3, 5 });
        expected.Add(new[] { -6, 1, 5 });
        List<int[]> output = ThreeNumberSumClass.ThreeNumberSum(new[] { 12, 3, 1, 2, -6, 5, -8, 6 }, 0);
        Assert.True(Compare(output, expected));
    }

    private bool Compare(List<int[]> triplets1, List<int[]> triplets2)
    {
        if (triplets1.Count != triplets2.Count) return false;
        for (int i = 0; i < triplets1.Count; i++)
        {
            if (!triplets1[i].SequenceEqual(triplets2[i]))
            {
                return false;
            }
        }
        return true;
    }
}