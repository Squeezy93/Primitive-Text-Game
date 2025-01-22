using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class AcceptGameCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public AcceptGameCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix => "accept_game";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            //извлечение айди игрока и оппонента
            var ids = update.CallbackQuery.Data.Substring(Prefix.Length + 1).Split('_');
            var userId = int.Parse(ids[0]);
            var opponentId = int.Parse(ids[1]);
            //получение юзеров из бд
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(userId));
            var opponent = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(opponentId));
            if (user is null || opponent is null) return false;
            //обновление состояния текущего юзера
            user.IsPlayingGame = true;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            //проверка состояния оппонента
            if (opponent.IsPlayingGame)
            {
                user.IsSearchingForGame = false;
                opponent.IsSearchingForGame = false;
                userRepository.Update(user);
                userRepository.Update(opponent);
                await userRepository.SaveChangesAsync();
                await botClient.SendMessage(user.UserTelegramId, "Game starting");
                await botClient.SendMessage(opponent.UserTelegramId, "Game starting");
            }
            else
            {
                await botClient.SendMessage(user.UserTelegramId, "Вы подтвердили участие. Ожидаем подтверждения от вашего оппонента.");
            }
            return true;
        }
    }
}
