namespace PrimitiveTextGame.Telegram.Modules.Common;

public static class ModuleExtensions
{
    private static readonly List<IModule> registeredModules = new();// мы сюда не добавили наши зарегестрированные модули

    public static IServiceCollection RegisterModules(this IServiceCollection services, IConfiguration configuration)
    {
        var modules = DiscoverModules();

        foreach (var module in modules)
        {
            module.RegisterModule(services, configuration);
        }

        return services;
    }

    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(x => x.IsClass && x.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}