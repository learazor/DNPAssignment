namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    //no argument constructor for EFC
    private Post() { }
    
    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }
}