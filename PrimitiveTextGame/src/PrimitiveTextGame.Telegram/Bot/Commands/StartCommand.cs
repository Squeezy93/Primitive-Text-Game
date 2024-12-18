using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Bot.Commands;

public class StartCommand : IBotCommand
{
	public string Prefix { get; } = "start";

	public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
	{
		if (update.Message == null || update.Message.Chat == null) return false;

		var responseText = "Привет! Добро пожаловать в текстовую игру. Введите команду для продолжения.";

        await botClient.SendMessage(update.Message.Chat.Id, responseText, ParseMode.Markdown);
		return true;
	}
}