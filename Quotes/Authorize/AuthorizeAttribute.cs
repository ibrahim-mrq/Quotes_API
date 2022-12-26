namespace Quotes.Authorize;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Quotes.Helper;
using Quotes.Models;

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
        var user = context.HttpContext.Items["User"];
        var token = context.HttpContext.Items["token"];

        if (user == null)
        {
            var result = new OperationType { Status = false, Message = "unauthorized", Code = 401, Data = null };
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new JsonResult(result);
        }
        else
        {
            var currentUser = (User)user;
            if (currentUser.Token != $"{token}")
            {
                var result = new OperationType { Status = false, Message = "Invaled token!", Code = 401, Data = null };
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new JsonResult(result);
            }
            if (currentUser.ExpirationToken < DateTime.Now)
            {
                var result = new OperationType { Status = false, Message = "Expiration token!", Code = 401, Data = null };
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new JsonResult(result);
            }
        }

    }
}
