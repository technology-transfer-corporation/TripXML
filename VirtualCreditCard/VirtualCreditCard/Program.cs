using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VirtualCreditCard.Extensions;
using VirtualCreditCard.Infrastructure.Context;
using VirtualCreditCard.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerService();

builder.Services.AddServices();

builder.Services.AddDbContext<VccDbContext>(options => options.UseInMemoryDatabase("UsersDatabase"));

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "VirtualCreditCard",
                      builder =>
                      {
                          builder.AllowAnyHeader();
                          builder.AllowAnyMethod();
                          builder.AllowAnyOrigin();
                      });
});

builder.AddJwtAuthentication();

builder.Services.AddOpenTracing();

builder.Services.AddTraceService();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Virtual Credit Card API");
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("VirtualCreditCard");
app.SeedDatabase();
app.Run();
