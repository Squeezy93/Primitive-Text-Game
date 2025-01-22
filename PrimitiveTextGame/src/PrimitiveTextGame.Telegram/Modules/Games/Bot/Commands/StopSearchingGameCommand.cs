using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class StopSearchingGameCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public StopSearchingGameCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix => "stop_searching";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            //получение юзера из бд
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(update.CallbackQuery.From.Id));
            //обновление статуса юзера
            user.IsSearchingForGame = false;
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();
            //оповещение юзера
            var inlineMarkupForUser = new InlineKeyboardMarkup()
                    .AddButton("Найти соперника", "search")
                    .AddNewRow()
                    .AddButton("Поменять персонажа", "change_player_character")
                    .AddNewRow()
                    .AddButton("Покинуть игру", "quit_game");
            await botClient.SendMessage(user.UserTelegramId, "Вы остановили поиск. Что хотите сделать?",
                replyMarkup: inlineMarkupForUser);
            return true;
        }
    }
}
