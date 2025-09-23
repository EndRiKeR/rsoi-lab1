using Microsoft.EntityFrameworkCore;
using Test.DataModels;

namespace DataBaseContext;

public sealed class Context : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;

    public Context(DbContextOptions<Context> dbContextOptions) : base(dbContextOptions)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}