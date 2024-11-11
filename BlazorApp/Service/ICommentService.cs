using DTOs;

namespace BlazorApp.Service;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task UpdateCommentAsync(int commentId, UpdateCommentDto request);
    Task RemoveCommentAsync(int commentId);
    Task<CommentDto> GetCommentByIdAsync(int commentId);
    Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId);
}