using Starly.Domain.Entities;
using Starly.Domain.Utilities;
using Customer.Domain.Dtos.Auth;
using Customer.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Infra.Data.Seeds._SeedHistory;

public class Seed_20240426152400_Add_Admin : Seed
{
    private readonly IAuthService authService;

    public Seed_20240426152400_Add_Admin(DbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
    {
        authService = serviceProvider.CreateScope().ServiceProvider.GetService<IAuthService>();
    }

    public override void Up()
    {
        var user = new RegisterDto
        {
            FirstName = "Admin",            
            Email = "admin@gmail.com",
            Password = "1q2w3e4r@#$A",
            LastName = "Admin",
            Username = "admin",
        };

        authService.RegisterAsync(user, StaticUserRoles.ADMIN).Wait();
    }
}
