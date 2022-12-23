using Quotes.Models;

namespace Quotes.Middlewares
{
    public interface IJwtUtils
    {
        public string GenerateToken(string Email, int Id);

        public string GenerateToken(User user);
        public int? IsValideteToken(string token);

    }
}
