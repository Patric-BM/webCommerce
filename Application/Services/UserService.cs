
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Validators;
using Infrastructure.Repositories;

namespace Application.Services;
public interface IUserService
{
    // User Authenticate(string username, string password);
    User GetById(int id);
    User Create(User user);
    User Update(User user);
    void Delete(int id);
}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidate<User> _userValidator;

    public UserService(IUserRepository userRepository, IValidate<User> userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }


    public User Create(User user)
    {
        var _errorMessages = _userValidator.Validate(user);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        _userRepository.CreateUser(user);
        return user;
    }

    public void Delete(int id)
    {
        var _errorMessages = _userValidator.Validate(_userRepository.GetUserById(id));
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        _userRepository.DeleteUser(id);
    }

    public User GetById(int id)
    {
        return _userRepository.GetUserById(id);
    }

    public User Update(User user)
    {
        var _errorMessages = _userValidator.Validate(user);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        _userRepository.UpdateUser(user);
        return user;
    }
}
