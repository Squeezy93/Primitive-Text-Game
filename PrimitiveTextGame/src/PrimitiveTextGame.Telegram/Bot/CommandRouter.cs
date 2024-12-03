using PrimitiveTextGame.Telegram.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Bot;

internal class CommandRouter
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
            throw new ArgumentException($"A command with prefix \"{command.Prefix}\" already exists.", nameof(command));
        }

        commands.Add(command);
    }

    public async Task<bool> Execute(ITelegramBotClient botClient, Update update)
    {
        if (update.Message.Type != MessageType.Text) return false;
        if (!update.Message.Text.StartsWith(globalPrefix)) return false;

        var command = commands.FirstOrDefault(cmd =>
            update.Message.Text.TrimStart(globalPrefix)
                .StartsWith(cmd.Prefix, StringComparison.InvariantCultureIgnoreCase));

        if (command == null)
        {
            return false;
        }
        else
        {
            return await command.ExecuteAsync(botClient, update).ConfigureAwait(false);
        }
    }
}