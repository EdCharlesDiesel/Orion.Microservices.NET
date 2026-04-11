using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class ValidIpAddressesClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            var input = "1921680";
            List<string> expected = new List<string>();
            expected.Add("1.9.216.80");
            expected.Add("1.92.16.80");
            expected.Add("1.92.168.0");
            expected.Add("19.2.16.80");
            expected.Add("19.2.168.0");
            expected.Add("19.21.6.80");
            expected.Add("19.21.68.0");
            expected.Add("19.216.8.0");
            expected.Add("192.1.6.80");
            expected.Add("192.1.68.0");
            expected.Add("192.16.8.0");
            var actual = ValidIpAddressesClass.ValidIpAddresses(input);
           
            Assert.True(expected.SequenceEqual(actual));
        }
    }
}