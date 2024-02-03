
using System;
using System.Text.Json;
using Domain.Responses;

namespace Domain.Exceptions;

public class NotFoundException : Exception, ICustomExceptionMiddleware
{
    public NotFoundException(string message) : base(message)
    {
    }

    public int StatusCode => 404;

    public string GetResponseMessage() => JsonSerializer.Serialize(new ErrorResponse(Message));
}