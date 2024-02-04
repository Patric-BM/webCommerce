
using System.Linq;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Mappers;
using Domain.Responses;
using Domain.Validators;
using Infrastructure.Repositories;

namespace Application.Services;
public interface IUserService
{
    // User Authenticate(string username, string password);
    UserResponse GetById(int id);

    UserResponse GetByEmail(string email);
    UserResponse Create(User user);
    UserResponse Update(User user);
    void Delete(int id);
}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidate<User> _userValidator;

    private readonly IHashingService _hashingService;

    public UserService(IUserRepository userRepository, IValidate<User> userValidator, IHashingService hashingService)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
        _hashingService = hashingService;
    }

    public UserResponse Create(User user)
    {
        var _errorMessages = _userValidator.Validate(user);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }

       user.PasswordHash = _hashingService.Hash(user.PasswordHash!  );

        _userRepository.CreateUser(user);
        return UserMapper.Map(user);
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

    public UserResponse GetById(int id)
    {
        return UserMapper.Map(_userRepository.GetUserById(id));
    }

    public UserResponse Update(User user)
    {
        var _errorMessages = _userValidator.Validate(user);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        _userRepository.UpdateUser(user);
        return UserMapper.Map(user);
    }

    public UserResponse GetByEmail(string email)
    {
        return UserMapper.Map(_userRepository.GetUserByEmail(email));
    }
}
