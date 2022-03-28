using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Suzan.Application.Helpers;
using Suzan.Domain.Model;

namespace Suzan.Application.Data;

public class DataContext : DbContext
{
    private readonly string _adminUsername;
    private readonly string _adminPassword;

    public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _adminUsername = configuration["Admin:Username"];
        _adminPassword = configuration["Admin:Password"];
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var (hash, salt) = PasswordHashHelper.HashPassword(_adminPassword);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Username = _adminPassword,
                Role = Role.Admin,
                PasswordHash = hash,
                PasswordSalt = salt
            }
        );
    }

    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}
