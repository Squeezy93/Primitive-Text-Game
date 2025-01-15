using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Common;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Bot;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Repositories;
using Telegram.Bot;

namespace PrimitiveTextGame.Telegram.Modules.Games;

public class GameModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("Primitive_text_game_bot")
            .RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient) =>
            {
                TelegramBotClientOptions options =
                    new TelegramBotClientOptions(configuration.GetSection("TelegramBot")["Token"]);
                return new TelegramBotClient(options, httpClient);
            });
            
        services.AddHostedService<BotProcess>();
        
        services.AddDbContext<ApplicationDataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnectionString")));
            
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<IArmorRepository, ArmorRepository>();
        services.AddScoped<IWeaponRepository, WeaponRepository>();        

        return services;
    }
}