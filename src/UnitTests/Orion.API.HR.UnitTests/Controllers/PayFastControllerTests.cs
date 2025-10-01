using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orion.API.HumanResources.Controllers;
using Orion.External.PaymentGateway.Controllers;

namespace Orion.API.HR.UnitTests.Controllers
{
    public class PayFastControllerTests
    {
        private PayFastController CreateController(Dictionary<string, string>? settings = null)
        {
            settings ??= new Dictionary<string, string>
            {
                { "PayFast:MerchantId", "10000100" },
                { "PayFast:MerchantKey", "46f0cd694581a" },
                { "PayFast:Passphrase", "test-pass" },
                { "PayFast:ReturnUrl", "https://example.com/return" },
                { "PayFast:NotifyUrl", "https://example.com/notify" },
                { "PayFast:CancelUrl", "https://example.com/cancel" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            return new PayFastController(configuration);
        }

        [Fact]
        public void CreatePayment_ReturnsRedirect_WithCorrectUrl()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.CreatePayment();

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.StartsWith("https://sandbox.payfast.co.za/eng/process?", redirectResult.Url);
            Assert.Contains("merchant_id=10000100", redirectResult.Url);
            Assert.Contains("merchant_key=46f0cd694581a", redirectResult.Url);
            Assert.Contains("amount=100.00", redirectResult.Url);
            Assert.Contains("item_name=Test+Product", redirectResult.Url);
        }

        [Fact]
        public void Notify_ReturnsOk()
        {
            // Arrange
            var controller = CreateController();

            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "payment_status", "COMPLETE" },
                { "amount_gross", "100.00" }
            });

            // Act
            var result = controller.Notify(formCollection);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Constructor_ThrowsIfMerchantIdMissing()
        {
            // Arrange
            var settings = new Dictionary<string, string>
            {
                { "PayFast:MerchantKey", "key" }
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CreateController(settings));
        }
    }
}
