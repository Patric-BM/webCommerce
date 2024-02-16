
using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Responses;
using Infrastructure.Repositories;

namespace Application.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> SignIn(Authentication authentication);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IJwtService _jwtService;
    private const string _message = "Invalid username or password";

    public AuthenticationService(IUserRepository userRepository, IHashingService hashingService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
        _jwtService = jwtService;
    }

    public async Task<AuthenticationResponse> SignIn(Authentication authentication)
    {
        Console.WriteLine(authentication.Email);
        Console.WriteLine(authentication.Password);
        var user = await _userRepository.GetUserByEmail(authentication.Email!);
        if (user is null)
        {
            throw new UnathorizedException(_message);
        }

        var isPasswordValid = _hashingService.Verify(authentication.Password!, user.PasswordHash!);

        if (!isPasswordValid)
        {
            throw new UnathorizedException(_message);
        }

        var jwt = _jwtService.CreateToken(user);
        

        return new AuthenticationResponse {
            Token = jwt,
        };


    }
}