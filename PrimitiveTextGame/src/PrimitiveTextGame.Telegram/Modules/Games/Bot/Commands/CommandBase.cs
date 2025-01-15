namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;

public abstract class CommandBase
{
    protected readonly IServiceScopeFactory ServiceScopeFactory;

    protected CommandBase(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
    }
}