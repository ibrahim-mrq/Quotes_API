namespace Quotes.Authorize;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var user = context.HttpContext.Items["Users"];
        if (user == null)
        {
            context.Result = new JsonResult(new
            {
                status = false,
                message = "unauthorized",
                code = 401,
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
