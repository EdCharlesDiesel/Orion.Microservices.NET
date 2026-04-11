using System;
using Microsoft.AspNetCore.Mvc;
using Orion.External.PaymentGateway.Model;

namespace Orion.External.PaymentGateway.Controllers
{
    [ApiController]
    [Route("api/paymentapprover")]
    public class PaymentApproverController : Controller
    {
        [HttpPost]
        public IActionResult TryPayment([FromBody] PaymentDto payment)
        {

            int num = new Random().Next(1000);
            if (num > 500)
                return Ok(true);

            return Ok(false);
        }
    }
}
