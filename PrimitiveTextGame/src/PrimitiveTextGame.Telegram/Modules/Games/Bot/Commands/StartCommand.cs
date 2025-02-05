using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;

public class StartCommand : ServiceScopeFactoryBase, IBotCommand
{
    public StartCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
    {

    }
    public string Prefix { get; } = "start";

    public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
    {
        if (update.Message == null || update.Message.Chat == null) return false;
        using var scope = ServiceScopeFactory.CreateAsyncScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        bool isUserExists = await userRepository.IsExists(new GetByUserTelegramIdSpecification(update.Message.From.Id));
        if (isUserExists)
        {
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(update.Message.From.Id));
            await notificationService.SendReturnMessage(user);
        }
        else
        {
            await notificationService.SendNewCharacterSelection(update.Message.From.Id);
        }
        return true;
    }
}