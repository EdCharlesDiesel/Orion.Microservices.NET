
namespace Orion.Helpers.UnitTests.Recursion
{
    
    public class ProductSumTests
    {
        [Fact(Skip = "Not Implemented yet")]
        public  void TestCase1()
        {
            List<object> test = new List<object>
            {
                1,
                2,
                new List<object>
                {
                    7,-1
                },
                3,
                 new List<object>
                 {
                     6,
                     new List<object>
                     {
                         -13,8
                     },
                     4
                 },
            };
            var result =  GenericClassAlgorithm.ProductSum(test);
            Assert.Equal(result, (8));
        }
    }
}