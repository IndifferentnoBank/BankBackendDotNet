using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Application.Dtos.Requests;
using UserService.Application.Services;
using UserService.Domain.Enums;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var user = await _userService.CreateUser(createUserDto);
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] CreateUserDto createUserDto)
    {
        var user = await _userService.UpdateUser(id, createUserDto);
        return Ok(user);
    }

    [HttpPost("lock-unlock/{id}")]
    public async Task<IActionResult> LockUnlockUser(Guid id, [FromQuery] bool isLocked)
    {
        await _userService.LockUnlockUser(id, isLocked);
        return Ok(new { Message = "User status updated." });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);
        return Ok(user);
    }

    [HttpGet("phone")]
    public async Task<IActionResult> GetUserByPhone([FromQuery] string phone)
    {
        var user = await _userService.GetUserByPhone(phone);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }
}
