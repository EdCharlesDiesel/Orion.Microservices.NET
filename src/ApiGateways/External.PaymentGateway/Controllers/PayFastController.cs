using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Orion.External.PaymentGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayFastController : ControllerBase
{
    private readonly string _merchantId;
    private readonly string _merchantKey;
    private readonly string _passphrase;
    private readonly string _returnUrl;
    private readonly string _notifyUrl;
    private readonly string _cancelUrl;

    public PayFastController(IConfiguration config)
    {
        _merchantId = config["PayFast:MerchantId"] ?? throw new ArgumentNullException("PayFast:MerchantId");
        _merchantKey = config["PayFast:MerchantKey"] ?? throw new ArgumentNullException("PayFast:MerchantKey");
        _passphrase = config["PayFast:Passphrase"] ?? "";
        _returnUrl = config["PayFast:ReturnUrl"] ?? "";
        _notifyUrl = config["PayFast:NotifyUrl"] ?? "";
        _cancelUrl = config["PayFast:CancelUrl"] ?? "";
    }

    [HttpGet("create")]
    public IActionResult CreatePayment()
    {
        var values = HttpUtility.ParseQueryString(string.Empty);
        values["merchant_id"] = _merchantId;
        values["merchant_key"] = _merchantKey;
        values["return_url"] = _returnUrl;
        values["cancel_url"] = _cancelUrl;
        values["notify_url"] = _notifyUrl;
        values["amount"] = "100.00";
        values["item_name"] = "Test Product";

        // TODO: Add signature check with _passphrase

        var url = "https://sandbox.payfast.co.za/eng/process?" + values.ToString();
        return Redirect(url);
    }

    [HttpPost("notify")]
    public IActionResult Notify([FromForm] IFormCollection form)
    {
        // TODO: Validate signatures, IP whitelisting, payment status
        // Save to DB / publish event
        return Ok();
    }
}