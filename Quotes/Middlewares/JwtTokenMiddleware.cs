using Quotes.Authorize;
using Quotes.Repositories.Interfaces;

namespace Quotes.Middlewares
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();
            int? userId = jwtUtils.IsValideteToken(token);
            if (userId == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                _ = context.Response.WriteAsJsonAsync(new
                {
                    status = false,
                    message = "You are festic, unauthorized",
                    code = 401,
                });
            }
            else
            {
                await _next(context);
            }
        }

    }
}
