using Microsoft.EntityFrameworkCore;
using Test.DataModels;

namespace DataBaseContext;

public sealed class Context : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;

    public Context(DbContextOptions<Context> dbContextOptions) : base(dbContextOptions)
    {
        try
        {
            Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection failed: {ex.Message}");
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}