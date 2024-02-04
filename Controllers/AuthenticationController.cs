
using System;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebCommerce.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _userService;
    public AuthenticationController(IAuthenticationService userService)
    {
        _userService = userService;

    }

    [HttpPost]
    public IActionResult SignIn( Authentication authentication)
    {
        var user = _userService.SignIn(authentication);
        Console.WriteLine(user);
        return Ok(user);
    }
}