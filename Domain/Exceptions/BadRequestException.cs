
using System;
using System.Collections.Generic;
using System.Text.Json;
using Domain.Responses;

namespace Domain.Exceptions;

public class BadRequestException : Exception, ICustomExceptionMiddleware
{
    private List<ErrorMessageResponses> Errors { get; }

    public int StatusCode => 400;

    public BadRequestException(List<ErrorMessageResponses> errors) : base()
    {
        Errors = errors;
    }

    public string GetResponseMessage() => JsonSerializer.Serialize(Errors);
}