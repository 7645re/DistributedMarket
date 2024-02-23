using Customers.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Customers.Domain;

public class CustomerDbContext : DbContext
{
    public DbSet<CustomerEntity> Customers { get; set; }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}