using Microsoft.IdentityModel.Tokens;
using Quotes.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quotes.Authorize
{
    public class JwtUtils : IJwtUtils
    {
        public string GenerateToken(string Email, int Id)
        {
            var key = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMonths(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Email , Email),
                        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                        new Claim("userId" , Id.ToString()),
                    }
                )
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMonths(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Email , user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                        new Claim("userId" , user.Id.ToString()),
                    }
                )
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public int? IsValideteToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var key = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // ClockSkew = TimeSpan.FromMinutes(1),
                    ClockSkew = TimeSpan.Zero,

                }, out SecurityToken validetedToken);

                var jwtToken = (JwtSecurityToken)validetedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }

    }
}
