
using System;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace WebCommerce.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        } 
        catch (Exception ex)
        {
            if (ex is ICustomExceptionMiddleware exception)
            {
                context.Response.StatusCode = exception.StatusCode;
                await context.Response.WriteAsync(exception.GetResponseMessage());
            }

        } 
    }
}