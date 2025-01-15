using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;

public class StartCommand : CommandBase, IBotCommand
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
        var characterRepository = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
        bool isUserExists = await userRepository.IsExists(new GetByUserTelegramIdSpecification(update.Message.From.Id));
        if (isUserExists)
        {
            var inlineMarkup = new InlineKeyboardMarkup()
                    .AddButton("Найти соперника", "search")
                    .AddNewRow()
                    .AddButton("Поменять персонажа", "change_player_character")
                    .AddNewRow()
                    .AddButton("Покинуть игру", "quit_game");

            var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(update.Message.From.Id));

            await botClient.SendMessage(update.Message.Chat.Id, $"Добро пожаловать обратно {user.UserName}. Ваш персонаж {user.Character.Name}. " +
                $"Что хотите сделать?", replyMarkup: inlineMarkup);
        }
        else
        {
            var inlineMarkup = new InlineKeyboardMarkup()
                .AddButton("Рыцарь", "create_player_Knight")
                .AddButton("Маг", "create_player_Mage")
                .AddButton("Лесоруб", "create_player_Lumberjack")
                .AddNewRow();
            await botClient.SendMessage(update.Message.Chat.Id, "Выбери героя!", replyMarkup: inlineMarkup);
        }
        return true;
    }
}