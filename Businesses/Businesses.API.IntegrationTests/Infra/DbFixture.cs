using Businesses.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Businesses.API.IntegrationTests.Infra;

public class DbFixture : IDisposable
{
    private readonly ApplicationDbContext _dbContext;

    public readonly string DatabaseName = $"starly_test_business_{Guid.NewGuid()}";
    public readonly string ConnectionString;

    private bool _disposed;

    public DbFixture()
    {
        ConnectionString = $"Server=host.docker.internal,1433;Database={DatabaseName};User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=false;TrustServerCertificate=true;MultipleActiveResultSets=true;";

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlServer(ConnectionString);

        _dbContext = new ApplicationDbContext(builder.Options);        
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _dbContext.Database.EnsureDeleted();

            _disposed = true;
        }
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DbFixture>
{
}