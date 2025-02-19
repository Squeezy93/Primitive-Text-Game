using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;
using PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot;

public class BotProcessWebhook
{
	private readonly CommandRouter _commandRouter;
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly IMemoryCache _memoryCache;

	public BotProcessWebhook(CommandRouter commandRouter,
		IServiceScopeFactory serviceScopeFactory,
		IMemoryCache cache)
	{
		_commandRouter = commandRouter ?? throw new ArgumentNullException(nameof(commandRouter));
		_serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
		_memoryCache = cache ??  throw new ArgumentNullException(nameof(cache));
		
		var cacheEntryOptions = new MemoryCacheEntryOptions
		{
			AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
			SlidingExpiration = null
		};
		_memoryCache.Set(TemplateConstants.MessagesTemplate, TemplateConstants.MessagesTemplateDictionary, cacheEntryOptions);
		_memoryCache.Set(TemplateConstants.ParametersTemplate, TemplateConstants.ParametersTemplatesDictionary, cacheEntryOptions);
		_memoryCache.Set(TemplateConstants.InlineMarkupTemplate, TemplateConstants.InlineMarkupTemplatesDictionary, cacheEntryOptions);
		
		using var scope = _serviceScopeFactory.CreateScope();
		var commands = scope.ServiceProvider.GetServices<IBotCommand>();
		foreach (var command in commands) 
		{ 
			_commandRouter.RegisterCommand(command);
		}
	}

	public async Task CallbackRoute(ITelegramBotClient botClient, Update update)
	{
		if (update.CallbackQuery == null) return;
		await UserLogic(botClient,update).ConfigureAwait(false);
	}

	public async Task MessageRoute(ITelegramBotClient botClient, Update update)
	{
		if (update.Message == null) return;
		await UserLogic(botClient, update).ConfigureAwait(false);
	}
	
	private async Task UserLogic(ITelegramBotClient botClient, Update update)
	{
		if (await _commandRouter.Execute(botClient, update).ConfigureAwait(false)) return;
	}
}