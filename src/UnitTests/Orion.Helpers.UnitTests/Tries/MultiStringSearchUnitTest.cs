namespace Orion.Helpers.UnitTests.Tries
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            bool[] expected = { true, false, true, true, false, true, false };
            var output = MultiStringSearch.MultistringSearchClass.MultistringSearch(
              "this is a big string",
              ["this", "yo", "is", "a", "bigger", "string", "kappa"]
            );
            Assert.True(Compare(output, expected));
        }

        public static bool Compare(List<bool> arr1, bool[] arr2)
        {
            if (arr1.Count != arr2.Length)
            {
                return false;
            }

            return !arr1.Where((t, i) => t != arr2[i]).Any();
        }
    }

    public class MultiStringSearch
    {
        public class MultistringSearchClass
        {
            public static List<bool> MultistringSearch(string thisIsABigString, string[] strings)
            {
                var result = new List<bool>();

                foreach (var str in strings)
                {
                    result.Add(thisIsABigString.Contains(str));
                }

                return result;
            }
        }
    }
}