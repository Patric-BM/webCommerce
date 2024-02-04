using System.Net;
using System.Text;
using Application.Services;
using Domain.Entities;
using Domain.Options;
using Domain.Validators;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebCommerce.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to configuration
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection(TokenOptions.Section));
builder.Services.Configure<PasswordHashOptions>(builder.Configuration.GetSection(PasswordHashOptions.Section));

builder.Services.AddCors(config =>
{
    config.AddPolicy("AllowOrigin", options => options
                                                 .AllowAnyOrigin()
                                                 .AllowAnyMethod());
});

var provider = builder.Services.BuildServiceProvider();
var tokenOptions = provider.GetRequiredService<IOptions<TokenOptions>>().Value;

var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key!));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = securityKey,
        ValidateIssuerSigningKey = true,

        ValidateAudience = true,
        ValidAudience = tokenOptions.Audience,
        ValidateIssuer = true,
        ValidIssuer = tokenOptions.Issuer,
        ValidateLifetime = true,
    };
});

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHashingService, HashingService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IValidate<Product>, ProductValidator>();
builder.Services.AddScoped<IValidate<User>, UserValidator>();
builder.Services.AddScoped<IValidate<Authentication>, AuthenticationValidator>();
builder.Services.AddScoped<IValidate<OrderItem>, PurchaseValidate>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchasesService, PurchasesService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
