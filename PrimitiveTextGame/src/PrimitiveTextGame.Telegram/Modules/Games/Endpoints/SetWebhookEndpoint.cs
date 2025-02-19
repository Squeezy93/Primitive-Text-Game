using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Telegram.Bot;

namespace PrimitiveTextGame.Telegram.Modules.Games.Endpoints;

public static class SetWebhookEndpoint
{
	public static void SetWebhook(this IEndpointRouteBuilder routeBuilder, IConfiguration configuration)
	{
		routeBuilder.MapGet("bot/setwebhook", async ([FromServices] TelegramBotClient bot) =>
		{
			string webhook = configuration.GetSection("TelegramBot")["WebhookUrl"];
			await bot.SetWebhook(webhook);
			return $"Webhook set to {webhook}";
		});
	}
}