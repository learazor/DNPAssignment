using DTOs;
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

        // Convert user to UserDto to avoid sending sensitive information
        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.Username
        };

        return Ok(userDto);
    }
}