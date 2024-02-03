
using System.Collections.Generic;
using Domain.Entities;
using Domain.Responses;

namespace Domain.Validators;

public class ProductValidator : IValidate<Product>
{
    public List<ErrorMessageResponses> Validate(Product entity)
    {
        var errors = new List<ErrorMessageResponses>();

        if (string.IsNullOrWhiteSpace(entity.Name))
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Name",
                Message = "Name is required"
            });
        }

        if (entity.Price <= 0)
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Price",
                Message = "Price must be greater than 0"
            });
        }

        return errors;
    }
}
