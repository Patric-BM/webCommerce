
using System.Collections.Generic;
using Domain.Entities;
using Domain.Responses;

namespace Domain.Validators;

public class UserValidator : IValidate<User>
{
    public List<ErrorMessageResponses> Validate(User entity)
    {
        var errors = new List<ErrorMessageResponses>();

        if (string.IsNullOrWhiteSpace(entity.Username))
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Username",
                Message = "Username is required"
            });
        }

        if (string.IsNullOrWhiteSpace(entity.PasswordHash))
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "PasswordHash",
                Message = "Password is required"
            });
        }

        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Email",
                Message = "Email is required"
            });
        }

        if (string.IsNullOrWhiteSpace(entity.Role))
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Role",
                Message = "Role is required"
            });
        }
        {
        }

        return errors;
    }
}