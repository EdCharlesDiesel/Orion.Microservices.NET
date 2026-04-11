using Orion.Helpers.Strings;

namespace Orion.Helpers.UnitTests.Strings
{
    public class CaesarCipherEncryptorClassUnitTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("zab", CaesarCipherEncryptorClass.CaesarCypherEncryptor("xyz", 2));
        }
    }
}