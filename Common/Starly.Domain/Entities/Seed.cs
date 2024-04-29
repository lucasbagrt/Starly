using Microsoft.EntityFrameworkCore;

namespace Starly.Domain.Entities;

public abstract class Seed(DbContext dbContext)
{
    public abstract void Up();
    protected string EnvironmentName { get; private set; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    protected DbContext DbContext { get; private set; } = dbContext;

    public static Seed CreateInstance(Type seedType, DbContext dbContext, IServiceProvider serviceProvider)
    {
        return Activator.CreateInstance(seedType, dbContext, serviceProvider) as Seed;
    }
}