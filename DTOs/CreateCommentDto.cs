namespace DTOs;

public class CreateCommentDto
{
    public string Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}