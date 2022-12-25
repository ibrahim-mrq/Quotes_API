using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quotes.Helper;
using Quotes.Repositories.Interfaces;
using Quotes.Repositories.other;
using Resturants.Repositories.other;
using System.Text;

namespace Quotes.Application
{
    public class BuilderServices
    {
        readonly WebApplicationBuilder builder;

        public BuilderServices(WebApplicationBuilder builder)
        {
            this.builder = builder;

        }

        public void AddScoped()
        {
            builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }

        public void AddAutoMapper()
        {
            builder.Services.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());
        }

        public void AddDataBase()
        {
            builder.Services.AddDbContext<DBContext>(
                opt => opt.UseSqlServer(
                    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Quotes;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
                    ));
        }

        public void JwtBearer()
        {
            var keyByte = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            builder.Services.AddAuthentication(op => op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(op =>
                {
                    op.SaveToken = true;
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(keyByte),
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };
                });
        }

    }
}
