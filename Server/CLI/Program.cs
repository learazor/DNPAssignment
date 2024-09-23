// See https://aka.ms/new-console-template for more information

using CLI.UI;
using RepositoryContracts;
using FileRepositories;

namespace CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IUserRepository userRepository = new UserFileRepository();
            IPostRepository postRepository = new PostFileRepository();
            ICommentRepository commentRepository = new CommentFileRepository();

            var app = new CliApp(userRepository, commentRepository, postRepository);
            await app.StartAsync();
        }
    }
}
