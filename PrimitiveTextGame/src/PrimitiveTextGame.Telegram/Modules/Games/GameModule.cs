using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Common;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Bot;
using PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Services;
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
        services.AddMemoryCache();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<IArmorRepository, ArmorRepository>();
        services.AddScoped<IWeaponRepository, WeaponRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IHistoryRepository, HistoryRepository>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ITurnService, TurnService>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IGameStateService, GameStateService>();
        services.AddScoped<IParametersTemplateService, ParametersTemplateService>();
        services.AddScoped<IMarkupTemplateService, MarkupTemplateService>();
        services.Scan(scan => scan.FromAssemblyOf<IBotCommand>()
            .AddClasses(classes => classes.AssignableTo<IBotCommand>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}