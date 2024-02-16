
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebCommerce.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]

public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> List()
    {
        var users = await _userService.List();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)!.Value) && !User.IsInRole("Admin"))
        {
            return Forbid();
        }
  
        var user = await _userService.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Create(User user)
    {
        var createdUser = _userService.Create(user);
        return Ok(createdUser);
    }

    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, User user)
    {
        user.Id = id;
        var updatedUser = await _userService.Update(user);
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
       await _userService.Delete(id);
        return NoContent();
    }

}