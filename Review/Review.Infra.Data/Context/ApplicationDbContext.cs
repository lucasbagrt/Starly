using Microsoft.EntityFrameworkCore;
using Review.Domain.Entities;
using Starly.Domain.Entities;

namespace Review.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<SeedHistory> SeedHistory { get; set; }
    public DbSet<ReviewPhoto> ReviewPhoto { get; set; }
    public DbSet<Domain.Entities.Review> Review { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
