using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(update.CallbackQuery.From.Id));
            user.IsSearchingForGame = true;
            userRepository.Update(user);
            var random = new Random();
            var opponent = (await userRepository.ListAsync(new FindUsersSearchingForGameSpecification(user))).OrderBy(op => random.Next()).FirstOrDefault();
            if (opponent is not null && !opponent.IsPlayingGame) 
            {
                var inlineMarkupForUser = new InlineKeyboardMarkup()
                    .AddButton("Да", $"accept_game_{user.UserTelegramId}_{opponent.UserTelegramId}")
                    .AddNewRow()
                    .AddButton("Нет", $"decline_game_{user.UserTelegramId}_{opponent.UserTelegramId}");
                await botClient.SendMessage(user.UserTelegramId, $"Противник найден. Вы играете против {opponent.UserName}. Готовы к битве?",
                    replyMarkup: inlineMarkupForUser);

                var inlineMarkupForOpponent = new InlineKeyboardMarkup()
                    .AddButton("Да", $"accept_game_{opponent.UserTelegramId}_{user.UserTelegramId}")
                    .AddNewRow()
                    .AddButton("Нет", $"decline_game_{opponent.UserTelegramId}_{user.UserTelegramId}");
                await botClient.SendMessage(opponent.UserTelegramId, $"Противник найден. Вы играете против {user.UserName}. Готовы к битве?",
                    replyMarkup: inlineMarkupForOpponent);
            }
            else
            {
                var inlineMarkupForUser = new InlineKeyboardMarkup()
                    .AddButton("Выйти из поиска", $"stop_searching_{user.UserTelegramId}");
                await botClient.SendMessage(user.UserTelegramId, $"Вы встали в поиск. Скоро противник найдется :)",
                    replyMarkup: inlineMarkupForUser);
            }
            await userRepository.SaveChangesAsync();
            return true;
        }
    }
}
