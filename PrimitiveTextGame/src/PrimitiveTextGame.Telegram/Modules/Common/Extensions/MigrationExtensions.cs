using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Data;

namespace PrimitiveTextGame.Telegram.Modules.Common.Extensions
{
    public static class MigrationExtensions 
    {
        public static async Task ApplyMigrationAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            
            var applicationDataContext = scope.ServiceProvider.GetService<ApplicationDataContext>(); 
            var characterRepository = scope.ServiceProvider.GetService<ICharacterRepository>();
            var armorRepository = scope.ServiceProvider.GetService<IArmorRepository>();
            var weaponRepository = scope.ServiceProvider.GetService<IWeaponRepository>();
            var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();
            
            var logger = loggerFactory?.CreateLogger(typeof(MigrationExtensions));
            
            try
            {
                
                await applicationDataContext.Database.MigrateAsync();
                logger?.LogInformation("Seeding data...");
                await DataSeed.SeedDataAsync(characterRepository, armorRepository, weaponRepository);
                logger?.LogInformation("Migrations applied!");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An error occurred during migrations or seeding data.");
            }
        }
    }
}

