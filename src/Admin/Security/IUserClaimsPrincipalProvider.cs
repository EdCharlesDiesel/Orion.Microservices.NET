using System.Security.Claims;

namespace Orion.Admin.Security
{
    public interface IUserClaimsPrincipalProvider
    {
        ClaimsPrincipal GetUser();
    }
}