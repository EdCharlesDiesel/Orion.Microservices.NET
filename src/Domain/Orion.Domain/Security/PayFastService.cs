using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Web;

namespace Orion.Domain.Security;

public class PayFastService
{
    private readonly PayFastOptions _options;

    public PayFastService(IOptions<PayFastOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Generates a PayFast redirect URL for a payment.
    /// </summary>
    public string CreatePaymentUrl(decimal amount, string itemName, string itemDescription = "")
    {
        var values = HttpUtility.ParseQueryString(string.Empty);

        values["merchant_id"] = _options.MerchantId;
        values["merchant_key"] = _options.MerchantKey;
        values["return_url"] = _options.ReturnUrl;
        values["cancel_url"] = _options.CancelUrl;
        values["notify_url"] = _options.NotifyUrl;
        values["amount"] = amount.ToString("F2");
        values["item_name"] = itemName;
        values["item_description"] = itemDescription;

        // TODO: Add passphrase signature generation if you want secure verification

        var url = "https://sandbox.payfast.co.za/eng/process?" + values.ToString();
        return url;
    }

    /// <summary>
    /// Handles PayFast Notify (IPN) requests.
    /// </summary>
    public Task HandleNotifyAsync(IDictionary<string, string> formValues)
    {
        // TODO:
        // 1. Validate source IP against PayFast IP ranges.
        // 2. Validate signature using passphrase.
        // 3. Post back to PayFast to verify data.
        // 4. Update DB or publish domain event.

        return Task.CompletedTask;
    }
}