using Businesses.API.IntegrationTests.Infra;
using Businesses.Infra.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Businesses.API.IntegrationTests.API.Configuration;

[Collection("Database")]
public class ApiApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    private readonly DbFixture _dbFixture;

    public ApiApplicationFactory(DbFixture dbFixture)
    {
        _dbFixture = dbFixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddSqlServer<ApplicationDbContext>(_dbFixture.ConnectionString);
        });
    }

    public HttpClient CreateClientWithNewDatabase()
    {
        var client = CreateClient();
        
        var scopeFactory = Services.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();            
            dbContext.Database.EnsureCreated();            
        }

        return client;
    }
}