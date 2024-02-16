
using System;
using System.Threading.Tasks;
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
    public async Task<IActionResult> SignIn( Authentication authentication)
    {
        var user = await _userService.SignIn(authentication);
        Console.WriteLine(user);
        return Ok(user);
    }
}