using PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot;

public class BotProcess
    : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<BotProcess> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private static readonly CommandRouter commandManager = new CommandRouter();

    public BotProcess(ITelegramBotClient telegramBotClient, ILogger<BotProcess> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _telegramBotClient = telegramBotClient ?? throw new ArgumentNullException(nameof(telegramBotClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        using var scope = _serviceScopeFactory.CreateScope();
        var commands = scope.ServiceProvider.GetServices<IBotCommand>();
        foreach (var command in commands) 
        { 
            commandManager.RegisterCommand(command);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _telegramBotClient.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync, new ReceiverOptions()
        {
            AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery]
        }, cancellationToken);
        _logger.LogInformation("Telegram bot started.");
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        switch (update.Type)
        {
            case UpdateType.Message:
                _logger.LogInformation("Processing message update..");
                await MessageRoute(botClient, update);
                break;
            case UpdateType.CallbackQuery:
                _logger.LogInformation("Proseccing callback query...");
                await CallbackRoute(botClient, update);
                break;
        }
    }

    private async Task CallbackRoute(ITelegramBotClient botClient, Update update)
    {
        if (update.CallbackQuery == null) return;
        await UserLogic(botClient,update).ConfigureAwait(false);
    }

    private async Task MessageRoute(ITelegramBotClient botClient, Update update)
    {
        if (update.Message == null) return;
        await UserLogic(botClient, update).ConfigureAwait(false);
    }

    private Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error has occured in bot service!");
        return Task.CompletedTask;
    }

    private async Task UserLogic(ITelegramBotClient botClient, Update update)
    {
        if (await commandManager.Execute(botClient, update).ConfigureAwait(false)) return;
    }
}