using EfcRepositories;
using EfcRepositories.repository;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Register AppContext with SQLite
builder.Services.AddDbContext<AppContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Register repository services
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();

// Add controllers
builder.Services.AddControllers();

// Add minimal session-based authentication
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession(); // Enable session middleware
app.UseAuthorization();
app.MapControllers();

app.Run();