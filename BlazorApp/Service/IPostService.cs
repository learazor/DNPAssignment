using DTOs;

namespace BlazorApp.Service;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task UpdatePostAsync(int postId, UpdatePostDto request);
    Task RemovePostAsync(int postId);
    Task<PostDto> GetPostByIdAsync(int postId);
    Task<List<PostDto>> GetPostsByUserIdAsync(int userId);
}