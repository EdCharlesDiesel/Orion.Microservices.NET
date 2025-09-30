using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class RunLengthEncodingClassUnitTest
    {

		[Fact]
		public void Method_Should_Return_False()
		{
            //ARRANGE
            var input = "AAAAAAAAAAAAABBCCCCDD";
            var expected = "9A4A2B4C2D";

            //ACT
            var actual = new RunLengthEncodingClass().RunLengthEncoding(input);

            //ASSERT
            Assert.Equal(expected, actual);					 
        }
    }
}