using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProjectMap.WebApi.Repositories;
using ProjectMap.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

builder.Services.AddTransient<IEnvironment2DRepository, Environment2DRepository>(o => new Environment2DRepository(sqlConnectionString));
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(o => new Object2DRepository(sqlConnectionString));
builder.Services.AddTransient<IUserRepository, UserRepository>(o => new UserRepository(sqlConnectionString));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddDapperStores(options => {options.ConnectionString = sqlConnectionString; });

builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options => { options.BearerTokenExpiration = TimeSpan.FromMinutes(60); });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

var app = builder.Build();

app.UseAuthorization();

app.MapGroup("/account").MapIdentityApi<IdentityUser>();

app.MapPost("/account/logout",
    async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) =>
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    }).RequireAuthorization();

// Configure the HTTP request pipeline.|
app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

app.MapGet("/", () => $"The API is up. Connection string found: {(sqlConnectionStringFound ? "Yes" : "No")}");