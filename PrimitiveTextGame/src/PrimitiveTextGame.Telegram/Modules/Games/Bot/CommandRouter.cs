using PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot;

public class CommandRouter
{
    private const char globalPrefix = '/';
    private readonly List<IBotCommand> commands = new List<IBotCommand>();

    public CommandRouter()
    {
    }

    public IBotCommand this[string prefix] => commands.FirstOrDefault(command => command.Prefix == prefix);

    public void RegisterCommand(IBotCommand command)
    {
        if (commands.Any(x => x.Prefix == command.Prefix))
        {
            throw new ArgumentException($"A commandFromMessage with prefix \"{command.Prefix}\" already exists.", nameof(command));
        }
        commands.Add(command);
    }

    public async Task<bool> Execute(ITelegramBotClient botClient, Update update)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message.Type != MessageType.Text) return false;
                if (!update.Message.Text.StartsWith(globalPrefix)) return false;

                var commandFromMessage = commands.FirstOrDefault(cmd =>
                    update.Message.Text.TrimStart(globalPrefix)
                        .StartsWith(cmd.Prefix, StringComparison.InvariantCultureIgnoreCase));
                return commandFromMessage != null && await commandFromMessage.ExecuteAsync(botClient, update).ConfigureAwait(false);

            case UpdateType.CallbackQuery:
                var callbackData = update.CallbackQuery?.Data;
                if(string.IsNullOrEmpty(callbackData)) return false;

                var commandFromCallback = commands.FirstOrDefault(cmd =>
                    callbackData.StartsWith(cmd.Prefix, StringComparison.InvariantCultureIgnoreCase));
                return commandFromCallback != null && await commandFromCallback.ExecuteAsync(botClient, update).ConfigureAwait(false);

            default: 
                return false;
        }
    }
}