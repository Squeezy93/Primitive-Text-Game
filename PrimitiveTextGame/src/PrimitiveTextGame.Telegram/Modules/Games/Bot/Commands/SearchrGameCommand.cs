using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class SearchrGameCommand : CommandBase, IBotCommand
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
            //начинаем процесс поиска игры
            user.IsSearchingForGame = true;
            userRepository.Update(user);            
            //ищем оппонента
            var random = new Random();
            var opponent = (await userRepository.ListAsync(new FindUsersSearchingForGameSpecification(user))).OrderBy(op => random.Next()).FirstOrDefault();
            if (opponent is not null) 
            {
                var inlineMarkup = new InlineKeyboardMarkup()
                    .AddButton("Да", "accept_battle")
                    .AddNewRow()
                    .AddButton("Нет", "decline_battle");

                await botClient.SendMessage(user.UserTelegramId, $"Противник найден. Вы играете против {opponent.UserName}. Готовы к битве?",
                    replyMarkup: inlineMarkup);
                await botClient.SendMessage(opponent.UserTelegramId, $"Противник найден. Вы играете против {opponent.UserName}. Готовы к битве?",
                    replyMarkup: inlineMarkup);
            }
            else
            {
                await botClient.SendMessage(user.UserTelegramId, $"Вы встали в поиск. Скоро противник найдется :)");
            }
            await userRepository.SaveChangesAsync();
            return true;
        }
    }
}
