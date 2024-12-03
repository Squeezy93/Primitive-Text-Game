using Telegram.Bot;
using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Bot.Commands;

public class StartCommand : IBotCommand
{
	public string Prefix { get; }
	public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
	{
		throw new NotImplementedException();
	}
}