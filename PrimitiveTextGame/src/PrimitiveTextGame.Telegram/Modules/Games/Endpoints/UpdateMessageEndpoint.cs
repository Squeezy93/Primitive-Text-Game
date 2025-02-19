using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PrimitiveTextGame.Telegram.Modules.Games.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Endpoints;

public static class UpdateMessageEndpoint
{
	public static void UpdateMessage(this IEndpointRouteBuilder routeBuilder)
	{
		routeBuilder.MapPost("/webhook",
			async (
				[FromBody] Update update, 
				[FromServices] ITelegramBotClient bot, 
				[FromServices] ILoggerFactory loggerFactory,
				[FromServices] BotProcessWebhook botProcess) =>
			{
				var logger = loggerFactory.CreateLogger(typeof(UpdateMessageEndpoint));
				
				switch (update.Type)
				{
					case UpdateType.Message:
						logger.LogInformation("Processing message update..");
						await botProcess.MessageRoute(bot, update);
						break;
					case UpdateType.CallbackQuery:
						logger.LogInformation("Processing callback query...");
						await botProcess.CallbackRoute(bot, update);
						break;
				}
			});
	}
}