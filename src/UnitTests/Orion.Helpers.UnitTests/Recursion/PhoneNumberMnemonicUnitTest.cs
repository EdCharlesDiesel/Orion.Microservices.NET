

using Orion.Helpers.Recursion;

namespace Orion.Helpers.UnitTests.Recursion
{
    public class PhoneNumberMnemonicTests
    {

        [Fact]
        public void Test1()
        {
            string phoneNumber = "1905";
            string[] expectedValues = new[] {
            "1w0j",
            "1w0k",
            "1w0l",
            "1x0j",
            "1x0k", 
            "1x0l",
            "1y0j",
            "1y0k",
            "1y0l",
            "1z0j",
            "1z0k",
            "1z0l"
        };
            List<string> expected = new List<string>();
            for (int i = 0; i < expectedValues.Length; i++)
            {
                expected.Add(expectedValues[i]);
            }
            var actual = PhoneNumberMnemonic.PhoneNumberMnemonics(phoneNumber);
            Assert.True(Enumerable.SequenceEqual(expected, actual));
        }
    }
}