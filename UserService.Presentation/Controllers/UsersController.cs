using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Dtos.Requests;
using UserService.Application.Services;

namespace UserService.Presentation.Controllers;

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
    //[HttpPost("login")]
    //public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    //{
    //    var userId = await _userService.LoginUser(loginUserDto);
    //    return Ok(new { userId = $"{userId}" });
    //}

    [HttpPut("{id}")]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] CreateUserDto createUserDto)
    {
        var user = await _userService.UpdateUser(JwtHelper.ExtractUserClaimsFromHeader(HttpContext).UserId, id,
            createUserDto);
        return Ok(user);
    }

    [HttpPost("lock-unlock/{id}")]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> LockUnlockUser(Guid id, [FromQuery] bool isLocked)
    {
        await _userService.LockUnlockUser(JwtHelper.ExtractUserClaimsFromHeader(HttpContext).UserId, id, isLocked);
        return Ok(new { Message = "User status updated." });
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(JwtHelper.ExtractUserClaimsFromHeader(HttpContext).UserId, id);
        return Ok(user);
    }

    [HttpGet("phone")]
    public async Task<IActionResult> GetUserByPhone([FromQuery] string phone)
    {
        var response = await _userService.GetUserByPhone(phone);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers(JwtHelper.ExtractUserClaimsFromHeader(HttpContext).UserId);
        return Ok(users);
    }
    
    [HttpGet("profile")]
    [Authorize(Policy = "CustomPolicy")]
    public async Task<IActionResult> GetUserProfile()
    {
        var user = await _userService.GetUserById(JwtHelper.ExtractUserClaimsFromHeader(HttpContext).UserId,
            JwtHelper.ExtractUserClaimsFromHeader(HttpContext).UserId);
        return Ok(user);
    }
    
}