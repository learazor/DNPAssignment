namespace WebAPI.Controllers;

using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto createCommentDto)
    {
        var comment = new Comment(createCommentDto.Body, createCommentDto.PostId, createCommentDto.UserId);
        var createdComment = await _commentRepository.AddAsync(comment);

        var commentDto = new CommentDto
        {
            Id = createdComment.Id,
            Body = createdComment.Body,
            PostId = createdComment.PostId,
            UserId = createdComment.UserId
        };

        return CreatedAtAction(nameof(GetCommentById), new { id = commentDto.Id }, commentDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetCommentById(int id)
    {
        var comment = await _commentRepository.GetSingleAsync(id);
        if (comment == null) return NotFound();

        var commentDto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        };

        return Ok(commentDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetAllComments([FromQuery] int? postId, [FromQuery] int? userId)
    {
        var comments = _commentRepository.GetMany();

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        var commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            PostId = c.PostId,
            UserId = c.UserId
        }).ToList();

        return Ok(commentDtos);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await _commentRepository.DeleteAsync(id);
        return NoContent();
    }
}

