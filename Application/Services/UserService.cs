
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    Task<UserResponse> GetById(int id);
    Task<List<UserResponse>> List();
    Task<UserResponse> GetByEmail(string email);
    Task<UserResponse> Create(User user);
    Task<UserResponse> Update(User user);
    Task Delete(int id);
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

    public  async Task<UserResponse> Create(User user)
    {
        var _errorMessages = _userValidator.Validate(user);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }

        user.PasswordHash = _hashingService.Hash(user.PasswordHash!);

        var createdUser = await _userRepository.CreateUser(user);
        return UserMapper.Map(createdUser);
    }

    public async Task Delete(int id)
    {
        var _errorMessages = _userValidator.Validate(await _userRepository.GetUserById(id));
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        await _userRepository.DeleteUser(id);
    }

    public async Task<UserResponse> GetById(int id)
    {
        return UserMapper.Map(await _userRepository.GetUserById(id));
    }

    public async Task<UserResponse> Update(User user)
    {
        var _errorMessages = _userValidator.Validate(user);
        if (_errorMessages.Any())
        {
            throw new BadRequestException(_errorMessages);
        }
        await _userRepository.UpdateUser(user);
        return UserMapper.Map(user);
    }

    public async Task<UserResponse> GetByEmail(string email)
    {
        return UserMapper.Map(await _userRepository.GetUserByEmail(email));
    }

    public async Task<List<UserResponse>> List()
    {
        return (await _userRepository.List()).Select(UserMapper.Map).ToList();
    }
}
