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
		
		using var scope = ServiceScopeFactory.CreateAsyncScope();
		var userRepository  = scope.ServiceProvider.GetRequiredService<IUserRepository>();
		//логика распознования игрока
		bool isUserExists = await userRepository.IsExists(new GetByUserTelegramId(update.Message.Chat.Id));

        if (isUserExists)
		{
			var inlineMarkup = new InlineKeyboardMarkup()
					.AddButton("Найти соперника", "search")
					.AddNewRow()
					.AddButton("Поменять персонажа", "change_player_character")
					.AddNewRow()
					.AddButton("Покинуть игру", "quit_game");

			/*var user = await userRepository.GetAsync(new GetByUserTelegramId(update.Message.Chat.Id));*/
			await botClient.SendMessage(update.Message.Chat.Id, $"Вы уже зарегестрированы. Что хотите сделать?",
				replyMarkup: inlineMarkup);
		}
		else
		{
			var inlineMarkup = new InlineKeyboardMarkup()
				.AddButton("Рыцарь", "create_player_knight")
				.AddNewRow()
				.AddButton("Маг", "create_player_mage")
				.AddNewRow()
				.AddButton("Лесоруб", "create_player_lumberjack")
				.AddNewRow();

			await botClient.SendMessage(update.Message.Chat.Id, "Выбери героя!",
				replyMarkup: inlineMarkup);
		}		
		return true;
	}
}