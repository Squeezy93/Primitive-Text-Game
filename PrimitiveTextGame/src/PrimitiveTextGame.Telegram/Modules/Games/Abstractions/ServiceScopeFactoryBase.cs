namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

public abstract class ServiceScopeFactoryBase
{
    protected readonly IServiceScopeFactory ServiceScopeFactory;

    protected ServiceScopeFactoryBase(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
    }
}