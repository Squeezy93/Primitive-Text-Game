using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Bot.Commands
{
    internal class CreatePlayerCommand : CommandBase, IBotCommand
    {
        public CreatePlayerCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix { get; } = "create";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            var character = new Character(update);
            var user = new Games.Models.User(update,character);

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var characterRepository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();

            characterRepository.Create(character);
            userRepository.Create(user);
            userRepository.SaveChangesAsync();

            var inlineMarkup = new InlineKeyboardMarkup()
                    .AddButton("Найти соперника", "search")
                    .AddNewRow()
                    .AddButton("Поменять персонажа", "change_player_character")
                    .AddNewRow()
                    .AddButton("Покинуть игру", "quit_game");

            await botClient.SendMessage(update.CallbackQuery.From.Id, $"Уважаемый {update.CallbackQuery.From.FirstName}! Ваш персонаж с классом" +
                $" {user.Character.Name} создан! У него {user.Character.Health} здоровья! Что хотите сделать?",
                replyMarkup: inlineMarkup);

            return true;
        }

    }
}
