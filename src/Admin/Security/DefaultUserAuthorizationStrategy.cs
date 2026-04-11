using System.Security.Claims;

namespace Orion.Admin.Security
{
    public class DefaultUserAuthorizationStrategy : IUserAuthorizationStrategy
    {
        private readonly SecurityUtility _securityUtility;

        public DefaultUserAuthorizationStrategy(
            IUserClaimsPrincipalProvider provider)
        {
            ClaimsPrincipal principal = provider.GetUser();

            _securityUtility =
                new SecurityUtility(principal.Identity, principal);
        }
        public bool IsAuthorizedForSearch
        {
            get
            {
                if (IsAdministrator())
                {
                    return true;
                }

                return _securityUtility.HasClaim(
                    SecurityConstants.Claim_SubscriptionType);
            }
        }

        private bool IsAdministrator()
        {
            return _securityUtility.IsInRole(
                SecurityConstants.RoleName_Admin);
        }

        // public bool IsAuthorizedForImages => throw new System.NotImplementedException();

        public bool IsAuthorizedForImages
        {
            get
            {
                if (IsAdministrator())
                {
                    return true;
                }

                return _securityUtility.HasClaim(
                    SecurityConstants.Claim_SubscriptionType, 
                    SecurityConstants.SubscriptionType_Ultimate);
            }
        }

    }
}