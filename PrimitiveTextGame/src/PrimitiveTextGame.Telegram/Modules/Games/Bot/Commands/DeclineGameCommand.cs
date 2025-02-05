using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class DeclineGameCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public DeclineGameCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix => "decline_game";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            //извлечение айди игрока и оппонента
            var ids = update.CallbackQuery.Data.Substring(Prefix.Length + 1).Split('_');
            var userId = int.Parse(ids[0]);
            var opponentId = int.Parse(ids[1]);
            //получение юзеров из бд
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(userId));
            var opponent = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(opponentId));
            if (user is null || opponent is null) return false;
            //обновление статуса юзера и оппонента
            user.IsSearchingForGame = false;
            opponent.IsSearchingForGame = true;
            opponent.IsPlayingGame = false;
            userRepository.Update(opponent);
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            //оповещение игроков о событии
            await notificationService.SendDeclineGame(userId);            
            await notificationService.SendReturnToSearch(opponentId);
            return true;
        }
    }
}
