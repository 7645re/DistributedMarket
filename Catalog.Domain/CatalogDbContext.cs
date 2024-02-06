using Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Domain;

public class CatalogDbContext : DbContext
{
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntityCategoryEntity> ProductCategory { get; set; }

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .Entity<ProductEntity>()
            .HasMany(p => p.Categories)
            .WithMany(p => p.Products)
            .UsingEntity<ProductEntityCategoryEntity>(j => j.ToTable("ProductCategory"));
    }
}