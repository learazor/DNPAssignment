using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories.repository;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        ctx.Comments.Add(comment);
        await ctx.SaveChangesAsync();
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await ctx.Comments.FindAsync(id);
        if (comment != null)
        {
            ctx.Comments.Remove(comment);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        return await ctx.Comments
            .Include(c => c.Post)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public IQueryable<Comment> GetMany()
    {
        return ctx.Comments
            .Include(c => c.Post)
            .Include(c => c.User)
            .AsQueryable();
    }
}