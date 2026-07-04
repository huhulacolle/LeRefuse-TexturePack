using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection.PortableExecutable;

namespace LeRefugeTexturePack.Server.Utils
{
    public class NoCacheFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var headers = context.HttpContext.Response.Headers;

            context.HttpContext.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers.Pragma = "no-cache";
            context.HttpContext.Response.Headers.Expires = "0";
            headers.Append("Accept-Ranges", "none");
        }
    }
}
