using Orion.Helpers.FamousAlgorithms;

namespace Orion.Helpers.UnitTests.Famous_Algorithms
{
    public class KruskalsAlgorithmClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = new[]
            {
              new[] { new[] { 1, 1 } }, new[] { new[] { 0, 1 } }
            };
            var expected = new[]
            {
              new[] { new[] { 1, 1 } }, new[] { new[] { 0, 1 } }
            };
            var actual = new KruskalsAlgorithmClass().KruskalsAlgorithm(input);
            Assert.True(jaggedArrayDeepEqual(expected, actual));
        }

        public bool jaggedArrayDeepEqual(dynamic a, dynamic b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            if (!a.GetType().IsArray || !b.GetType().IsArray)
            {
                return Equals(a, b);
            }

            if (a.Length == b.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (!jaggedArrayDeepEqual(a[i], b[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}