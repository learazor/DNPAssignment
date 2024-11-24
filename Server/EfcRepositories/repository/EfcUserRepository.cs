using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories.repository;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;

    public EfcUserRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await ctx.Users.FindAsync(id);
        if (user != null)
        {
            ctx.Users.Remove(user);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task<User> GetSingleAsync(int id)
    {
        return await ctx.Users
            .Include(u => u.Posts)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public IQueryable<User> GetMany()
    {
        return ctx.Users
            .Include(u => u.Posts)
            .AsQueryable();
    }

    public async Task<User> FindUserAsync(string requestUserName)
    {
        return await ctx.Users
            .FirstOrDefaultAsync(u => u.Username == requestUserName);
    }
}