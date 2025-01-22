using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Games.Data;

namespace PrimitiveTextGame.Telegram.Modules.Common.Extensions
{
    public static class MigrationExtensions 
    {
        public static async Task<IServiceCollection> ApplyMigrationAsync(this IServiceCollection services, IServiceScopeFactory serviceScopeFactory)
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDataContext>>();
            try
            {
                var applicationDataContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
                await applicationDataContext.Database.MigrateAsync();
                logger.LogInformation("Seeding data...");
                await DataSeed.SeedDataAsync(serviceScopeFactory);
                logger.LogInformation("Migrations applied!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migrations or seeding data.");
            }
            return services;
        }
    }
}

