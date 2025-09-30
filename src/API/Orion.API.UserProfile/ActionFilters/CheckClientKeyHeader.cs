using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.API.UserProfile.ActionFilters
{
    public class CheckClientKeyHeader : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // if the header has a client key is missing or set to false, 
            // a BadRequest must be returned.
            if (!context.HttpContext.Request.Headers
           .ContainsKey("_clientKey"))
            {
                context.Result = new BadRequestResult();
            }

            // get the ShowStatistics header 
            if (!bool.TryParse(
                    context.HttpContext.Request.Headers["_clientKey"].ToString(),
                    out bool showStatisticsValue))
            {
                context.Result = new BadRequestResult();
            }

            // check the value
            if (!showStatisticsValue)
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
