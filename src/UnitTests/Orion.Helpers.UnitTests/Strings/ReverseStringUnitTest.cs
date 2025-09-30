using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class ReverseStringClassUnitTest1
    {
		[Fact(Skip="Not implemented yet")]
        public void Method_Should_Return_False()
		{
			//ARRANGE
						 

			//ACT
			var actual = ReverseStringClass.ReverseString("abcde");

			//ASSERT					 
			Assert.Equal("edcba", actual);
		}


	}
}