using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userRepository.FindUserAsync(request.UserName);
        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Invalid username or password");
        }

        // Store user ID in the session
        HttpContext.Session.SetInt32("UserId", user.Id);

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.Username
        };

        return Ok(userDto);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear the session
        return Ok("Logged out successfully");
    }

    [HttpGet("current-user")]
    public IActionResult GetCurrentUser()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return Unauthorized("No user is logged in");
        }

        return Ok(new { UserId = userId });
    }
}