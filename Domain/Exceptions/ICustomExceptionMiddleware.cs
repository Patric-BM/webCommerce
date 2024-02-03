
namespace Domain.Exceptions;

public interface ICustomExceptionMiddleware {

    public int StatusCode { get; }
    public string GetResponseMessage();

}