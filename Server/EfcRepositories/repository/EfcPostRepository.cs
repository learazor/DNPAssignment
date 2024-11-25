using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories.repository;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post post)
    {
        ctx.Posts.Add(post);
        await ctx.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var post = await ctx.Posts.FindAsync(id);
        if (post != null)
        {
            ctx.Posts.Remove(post);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        return await ctx.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public IQueryable<Post> GetMany()
    {
        return ctx.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .AsQueryable();
    }
}