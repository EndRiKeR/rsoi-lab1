using Microsoft.EntityFrameworkCore;
using RsoiLab1.DataModels;

namespace DataBaseContext;

public class Context : DbContext
{
    public virtual DbSet<Person> Persons { get; set; }

    public Context() { }
    public Context(DbContextOptions<Context> dbContextOptions) : base(dbContextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}