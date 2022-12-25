using Microsoft.TeamFoundation.TestManagement.WebApi;
using Quotes.Application;
using Quotes.Authorize;
using Quotes.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors();
// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(x => x.Filters.Add<AuthorizeAttribute>());
BuilderServices builderServices = new(builder);
builderServices.AddScoped();
builderServices.AddDataBase();
builderServices.AddAutoMapper();
builderServices.JwtBearer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();
//app.UseAuthentication();
app.UseMiddleware<JwtTokenMiddleware>();

app.MapControllers();

app.Run();


