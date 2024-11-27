using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Common;
using PrimitiveTextGame.Telegram.Modules.Game.Data;

namespace PrimitiveTextGame.Telegram.Modules.Games;

public class GameModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnectionString")));

        return services;
    }
}