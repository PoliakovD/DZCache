using DZCache.Models;
using Microsoft.EntityFrameworkCore;

namespace DZCache.Data;

public class DzDbContext:DbContext
{
    public DbSet<Product> Products { get; set; }

    public DzDbContext() :base()
    {
    }
    public DzDbContext(DbContextOptions<DzDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasData([
                new Product(Guid.NewGuid(),$"{Guid.NewGuid():N}",123),
                new Product(Guid.NewGuid(),$"{Guid.NewGuid():N}",123),
                new Product(Guid.NewGuid(),$"{Guid.NewGuid():N}",123),
                new Product(Guid.NewGuid(),$"{Guid.NewGuid():N}",123)
            ]);
        base.OnModelCreating(modelBuilder);
    }
}