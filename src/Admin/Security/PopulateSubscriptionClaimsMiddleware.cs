using System.Security.Claims;
using Orion.Admin.Controllers;

namespace Orion.Admin.Security
{
    public class PopulateSubscriptionClaimsMiddleware(ISubscriptionService subscriptionServiceInstance) : IMiddleware
    {
        private ISubscriptionService _subscriptionService = subscriptionServiceInstance;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User != null &&
                context.User.HasClaim(
                    c => c.Type == ClaimTypes.Name))
            {
                var usernameClaim = 
                    context.User.Claims.Where(
                        c => c.Type == ClaimTypes.Name).FirstOrDefault();

                var username = usernameClaim.Value;
                
                var subscriptionType =
                    _subscriptionService.GetSubscriptionType(
                        username);                

                if (subscriptionType != null)
                {
                    var claims = new List<Claim>();

                    // copy the existing claims
                    claims.AddRange(context.User.Claims);

                    AddClaim(claims, 
                        SecurityConstants.Claim_SubscriptionType, 
                        subscriptionType);

                    var identity = new ClaimsIdentity(claims);

                    context.User = new ClaimsPrincipal(identity);
                }
            }
                        
            await next(context);
        }

        private static void AddClaim(List<Claim> claims, string claimTypeName, string value)
        {
            claims.Add(new Claim(claimTypeName, value));
        }
    }
}