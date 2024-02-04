
using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Responses;

namespace Domain.Validators;

public class PurchaseValidate : IValidate<OrderItem>
{
    public List<ErrorMessageResponses> Validate(OrderItem entity)
    {
        var errors = new List<ErrorMessageResponses>();

        if (entity.products.Count == 0)
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Products",
                Message = "Products are required"
            });
        }

        if (entity.Date == DateTime.MinValue)
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "Date",
                Message = "Date is required"
            });
        }

        if (entity.TotalPrice <= 0)
        {
            errors.Add(new ErrorMessageResponses
            {
                Field = "TotalPrice",
                Message = "Total Price is required"
            });
        }

        return errors;
    }
}