
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebCommerce.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        var createdUser = _userService.Create(user);
        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, User user)
    {
        user.Id = id;
        var updatedUser = _userService.Update(user);
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return NoContent();
    }

}