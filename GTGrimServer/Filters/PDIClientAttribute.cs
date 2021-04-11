using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Filters
{
    /// <summary>
    /// Verifies that the user agent comes from a proper game client.
    /// </summary>
    /// <returns></returns>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PDIClientAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userAgent = context.HttpContext.Request.Headers["User-Agent"];
            if (StringValues.IsNullOrEmpty(userAgent))
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (userAgent.Count != 1 || !userAgent[0].Equals(GTConstants.PDIUserAgent))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
