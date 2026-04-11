using System;
using System.Security.Cryptography;
using System.Text;

public class OzowService
{
    private readonly string _siteCode = "YourSiteCode";
    private readonly string _privateKey = "YourPrivateKey";

    public string GenerateHash(string siteCode, string countryCode, string amount, string transactionReference, string bankReference, string notifyUrl, string returnUrl, string cancelUrl)
    {
        var data = $"{siteCode}{countryCode}{amount}{transactionReference}{bankReference}{notifyUrl}{returnUrl}{cancelUrl}";

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_privateKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}