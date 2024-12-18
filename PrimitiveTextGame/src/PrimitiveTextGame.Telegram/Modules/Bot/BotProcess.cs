using PrimitiveTextGame.Telegram.Modules.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Bot;

public class BotProcess
    : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<BotProcess> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private static readonly CommandRouter commandManager = new CommandRouter();

    public BotProcess(ITelegramBotClient telegramBotClient, ILogger<BotProcess> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        commandManager.RegisterCommand(new StartCommand(serviceScopeFactory));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _telegramBotClient.StartReceiving(HandlerUpdateAsync, HandlerErrorAsync, new ReceiverOptions(), cancellationToken);
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
            //case UpdateType.CallbackQuery:
            //Домашка создать маршрут под callback и обработать создание игрока с нужным типом    
        }
    }

    private Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error has occured in bot service!");
        return Task.CompletedTask;
    }

    private static async Task MessageRoute(ITelegramBotClient botClient, Update update)
    {
        if (update.Message == null) return;

        await UserLogic(botClient, update).ConfigureAwait(false);
    }

    private static async Task UserLogic(ITelegramBotClient botClient, Update update)
    {
        if (await commandManager.Execute(botClient, update).ConfigureAwait(false)) return;
    }
}