using Businesses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Starly.Domain.Entities;

namespace Businesses.Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Business> Business { get; set; }
    public DbSet<BusinessPhoto> BusinessPhoto { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<SeedHistory> SeedHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
