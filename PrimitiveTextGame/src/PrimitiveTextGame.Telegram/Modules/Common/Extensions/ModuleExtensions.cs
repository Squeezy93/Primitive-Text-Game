using Microsoft.AspNetCore.Builder;

namespace PrimitiveTextGame.Telegram.Modules.Common.Extensions;

public static class ModuleExtensions
{
    private static readonly List<IModule> _modules = new (); 
    public static IServiceCollection RegisterModules(this IServiceCollection services, IConfiguration configuration)
    {
        var modules = DiscoverModules();
        foreach (var module in modules)
        {
            module.RegisterModule(services, configuration);
            _modules.Add(module);
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

    public static WebApplication MapEndpoints(this WebApplication app, IConfiguration configuration)
    {
        foreach (var module in _modules)
        {
            module.MapEndpoints(app, configuration);
        }

        return app;
    }
}