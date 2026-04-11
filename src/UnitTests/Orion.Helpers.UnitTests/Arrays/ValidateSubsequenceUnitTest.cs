

using Orion.Helpers.Arrays;

namespace Orion.Helpers.UnitTests.Arrays
{
    public class ValidateSubsequenceUnitTest
    {
        [Fact]
        public void TestCase1()
        {
            List<int> array = new List<int> {
                5, 1, 22, 25, 6, -1, 8, 10
            };

            List<int> sequence = new List<int> {
            1, 6, -1, 10
            };

          Assert.True(ValidateSubsequenceClass.IsValidSubsequence(array, sequence));
        }
    }

 
}

   