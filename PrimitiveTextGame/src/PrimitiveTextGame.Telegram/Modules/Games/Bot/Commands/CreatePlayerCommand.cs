using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.ArmorSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.CharacterSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.WeaponSpecifications;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class CreatePlayerCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public CreatePlayerCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix { get; } = "create_player";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var user = await userService.CreateNewUser(update);
            await notificationService.SendUserCreated(user);
            return true;
        }
    }
}
