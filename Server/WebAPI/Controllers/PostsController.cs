namespace WebAPI.Controllers;

using DTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        var post = new Post(createPostDto.Title, createPostDto.Body, createPostDto.UserId);
        var createdPost = await _postRepository.AddAsync(post);

        var postDto = new PostDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            UserId = createdPost.UserId
        };

        return CreatedAtAction(nameof(GetPostById), new { id = postDto.Id }, postDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPostById(int id)
    {
        var post = await _postRepository.GetSingleAsync(id);
        if (post == null) return NotFound();

        var postDto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        };

        return Ok(postDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetAllPosts([FromQuery] string? title, [FromQuery] int? userId)
    {
        var posts = _postRepository.GetMany();

        if (!string.IsNullOrEmpty(title))
        {
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        var postDtos = posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId
        }).ToList();

        return Ok(postDtos);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        await _postRepository.DeleteAsync(id);
        return NoContent();
    }
}
