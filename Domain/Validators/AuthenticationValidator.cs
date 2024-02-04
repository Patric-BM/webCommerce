
using System.Collections.Generic;
using Domain.Entities;
using Domain.Responses;

namespace Domain.Validators;

public class AuthenticationValidator : IValidate<Authentication>
{
    public List<ErrorMessageResponses> Validate(Authentication entity)
    {
        var errors = new List<ErrorMessageResponses>();

        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Email",
                Message = "Email is required"
            });
        }

        if (string.IsNullOrWhiteSpace(entity.Password))
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Password",
                Message = "Password is required"
            });
        }

        return errors;
    }
}
