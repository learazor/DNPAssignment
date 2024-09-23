using Entities;

using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Post");
            Console.WriteLine("3. Add Comment");
            Console.WriteLine("4. View Posts Overview");
            Console.WriteLine("5. View Specific Post");
            Console.WriteLine("6. Exit");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                case "2":
                    await CreatePostAsync();
                    break;
                case "3":
                    await AddCommentAsync();
                    break;
                case "4":
                    ViewPostsOverview();
                    break;
                case "5":
                    await ViewSpecificPostAsync();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("You can choose options from 1 to 6.");
                    break;
            }
        }
    }

    private async Task CreateUserAsync()
    {
        Console.Write("Enter username: ");
        var username = Console.ReadLine();
        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        var user = new User(username, password);
        await _userRepository.AddAsync(user);
        Console.WriteLine($"User created with ID: {user.Id}");
    }

    private async Task CreatePostAsync()
    {
        Console.Write("Enter title: ");
        var title = Console.ReadLine();
        Console.Write("Enter body: ");
        var body = Console.ReadLine();
        Console.Write("Enter user ID: ");
        if (!int.TryParse(Console.ReadLine(), out var userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        var post = new Post(title, body, userId);
        await _postRepository.AddAsync(post);
        Console.WriteLine($"Post created with ID: {post.Id}");
    }

    private async Task AddCommentAsync()
    {
        Console.Write("Enter comment: ");
        var body = Console.ReadLine();
        Console.Write("Enter user ID: ");
        if (!int.TryParse(Console.ReadLine(), out var userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }
        Console.Write("Enter post ID: ");
        if (!int.TryParse(Console.ReadLine(), out var postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }

        var comment = new Comment(body, userId, postId);
        await _commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment added to Post ID: {postId}");
    }

    private void ViewPostsOverview()
    {
        var posts = _postRepository.GetMany();
        if (!posts.Any())
        {
            Console.WriteLine("No posts available.");
            return;
        }

        foreach (var post in posts)
        {
            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
        }
    }

    private async Task ViewSpecificPostAsync()
    {
        Console.Write("Enter post ID: ");
        if (!int.TryParse(Console.ReadLine(), out var postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }

        var post = await _postRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");

        var comments = _commentRepository.GetMany().Where(c => c.PostId == postId);
        if (!comments.Any())
        {
            Console.WriteLine("No comments for this post.");
        }
        else
        {
            foreach (var comment in comments)
            {
                Console.WriteLine($"Comment by User {comment.UserId}: {comment.Body}");
            }
        }
    }
}




    
