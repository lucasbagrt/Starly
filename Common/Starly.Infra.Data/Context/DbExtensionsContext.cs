using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Starly.Infra.Data.Context;

public static class DbExtensionsContext
{
    public static bool AllMigrationsApplied(this DbContext context)
    {
        Console.WriteLine("AllMigrationsApplied");

        var applied = context.GetService<IHistoryRepository>()
            .GetAppliedMigrations()
            .Select(m => m.MigrationId);

        var total = context.GetService<IMigrationsAssembly>()
            .Migrations
            .Select(m => m.Key);

        return !total.Except(applied).Any();
    }
}
