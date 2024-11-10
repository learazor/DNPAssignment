using DTOs;

namespace BlazorApp.Service;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task UpdateUserAsync(int id, UpdateUserDto request);
    public Task<bool> RemoveUserAsync(int userId);
}
