using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Games.Data;

namespace PrimitiveTextGame.Telegram.Modules.Common
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrationAsync(this IServiceCollection services, IServiceScopeFactory serviceScopeFactory)
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDataContext>>();
            try
            {
                var applicationDataContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
                await applicationDataContext.Database.MigrateAsync();

                await DataSeed.SeedDataAsync(services, serviceScopeFactory);
                logger.LogInformation("Migrations applied!");
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "An error occurred during migrations or seeding data.");
            }
        }
    }
}
