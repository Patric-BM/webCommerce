using Application.Services;
using Domain.Entities;
using Domain.Options;
using Domain.Validators;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCommerce.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to configuration
builder.Services.Configure<ClassOptions>(builder.Configuration.GetSection(ClassOptions.Section));

builder.Services.AddCors(config =>
{
    config.AddPolicy("AllowOrigin", options => options
                                                 .AllowAnyOrigin()
                                                 .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidate<Product>, ProductValidator>();
builder.Services.AddScoped<IValidate<User>, UserValidator>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");
app.UseAuthorization();
app.MapControllers();
app.Run();
