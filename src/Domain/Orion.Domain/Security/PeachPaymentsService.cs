using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orion.Domain.Security;

public class PeachPaymentsService
{
    private readonly HttpClient _httpClient;
    private readonly string _username = "your-entity-id";
    private readonly string _password = "your-api-key";

    public PeachPaymentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{_password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
    }

    public async Task<string> CreatePaymentAsync(decimal amount, string cardNumber, string expiry, string cvv)
    {
        var payload = new
        {
            amount = amount.ToString("F2"),
            currency = "ZAR",
            paymentBrand = "VISA",
            paymentType = "DB",
            card = new { number = cardNumber, holder = "JOHN DOE", expiryMonth = "12", expiryYear = "2025", cvv = cvv }
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://test.oppwa.com/v1/payments", content);
        return await response.Content.ReadAsStringAsync();
    }
}