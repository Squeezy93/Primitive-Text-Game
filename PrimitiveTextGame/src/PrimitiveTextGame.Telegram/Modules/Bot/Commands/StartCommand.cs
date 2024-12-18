using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Bot.Commands;

public class StartCommand : CommandBase, IBotCommand
{
	public StartCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
	{
		
	}
	public string Prefix { get; } = "start";

	public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
	{
		if (update.Message == null || update.Message.Chat == null) return false;

		var responseText = "Привет! Добро пожаловать в текстовую игру. Введите команду для продолжения.";
		
		using var scope = ServiceScopeFactory.CreateAsyncScope();
		var userRepository  = scope.ServiceProvider.GetRequiredService<IUserRepository>();
		//логика распознования игрока
		bool isUserExists = await userRepository.IsExists(new GetByUserTelegramId(update.Message.Chat.Id));
		
		if (isUserExists)
		{
			var inlineMarkup = new InlineKeyboardMarkup()
					.AddButton("Найти соперника", "search");

			await botClient.SendMessage(update.Message.Chat.Id, "Выбери героя!",
				replyMarkup: inlineMarkup);
		}
		else
		{
			/*
			 * Knight,
			   Mage,
			   Lumberjack
			 */
			var inlineMarkup = new InlineKeyboardMarkup()
				.AddButton("Рыцарь", "player_knight")
				.AddNewRow()
				.AddButton("Маг", "player_mage")
				.AddNewRow()
				.AddButton("Лесоруб", "player_lumberjack")
				.AddNewRow();

			await botClient.SendMessage(update.Message.Chat.Id, "Выбери героя!",
				replyMarkup: inlineMarkup);
		}
		
		return true;
	}
}