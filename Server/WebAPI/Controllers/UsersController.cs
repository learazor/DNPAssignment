﻿namespace WebAPI.Controllers;

using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var user = new User(createUserDto.UserName, createUserDto.Password);
        var createdUser = await _userRepository.AddAsync(user);

        var userDto = new UserDto
        {
            Id = createdUser.Id,
            UserName = createdUser.Username
        };

        return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await _userRepository.GetSingleAsync(id);
        if (user == null) return NotFound();

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.Username
        };

        return Ok(userDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAllUsers([FromQuery] string? username)
    {
        var users = _userRepository.GetMany();

        if (!string.IsNullOrEmpty(username))
        {
            users = users.Where(u => u.Username.Contains(username));
        }

        var userDtos = users.Select(u => new UserDto { Id = u.Id, UserName = u.Username }).ToList();
        return Ok(userDtos);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteAsync(id);
        return NoContent();
    }
}