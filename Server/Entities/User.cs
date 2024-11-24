namespace Entities;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    //no argument constructor for EFC
    private User() { }

    public User(string? username, string password)
    {
        Username = username;
        Password = password;
    }
}