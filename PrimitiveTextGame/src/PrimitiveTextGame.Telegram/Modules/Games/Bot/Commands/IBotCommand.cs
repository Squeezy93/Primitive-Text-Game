using Telegram.Bot;
using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;

public interface IBotCommand
{
    string Prefix { get; }
    Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update);
}