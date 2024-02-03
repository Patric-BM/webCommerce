
using System.Collections.Generic;
using Domain.Responses;

namespace Domain.Validators;

public interface IValidate<T>
{
    List<ErrorMessageResponses> Validate(T entity);
}