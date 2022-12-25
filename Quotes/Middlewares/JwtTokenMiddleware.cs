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

        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            var token = context.Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();
            int? userId = userRepository.IsValideteToken($"{token}");
            if (userId != null)
            {
                context.Items["User"] = userRepository.GetUserById(userId.Value);
                context.Items["token"] = token;
                context.Items["userId"] = userId;
            }
            await _next(context);
        }

    }
}
