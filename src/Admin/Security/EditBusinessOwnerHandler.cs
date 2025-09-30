using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.Admin.Security
{
    public class EditBusinessOwnerHandler :
        AuthorizationHandler<EditBusinessOwnerRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            EditBusinessOwnerRequirement requirement)
        {
            if (context.Resource is 
                AuthorizationFilterContext mvcContext)
            {
                if (mvcContext.RouteData.Values.ContainsKey("id") == false)
                {
                    context.Fail();
                }
                else
                {
                    int id = Convert.ToInt32(mvcContext.RouteData.Values["id"]);

                    var utility = new SecurityUtility(context.User.Identity, context.User);

                    if (utility.IsAuthorized(SecurityConstants.PermissionName_Edit, id))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}