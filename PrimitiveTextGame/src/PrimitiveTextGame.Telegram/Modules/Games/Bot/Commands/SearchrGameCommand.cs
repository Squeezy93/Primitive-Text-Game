using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class SearchrGameCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public SearchrGameCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix => "search";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();            
            var user = await userService.StartSearchingForGame(update.CallbackQuery.From.Id);
            var opponent = await userService.FindOpponent(user);
            if (opponent is not null)
            {
                await notificationService.SendGameInvitation(user, opponent);
            }
            else
            {
                await notificationService.SendStartSearching(user.UserTelegramId);                
            }
            return true;
        }
    }
}
