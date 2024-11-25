namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }

    public int PostId { get; set; }

    public Post Post { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    //no argument constructor for EFC
    private Comment() { }

    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}