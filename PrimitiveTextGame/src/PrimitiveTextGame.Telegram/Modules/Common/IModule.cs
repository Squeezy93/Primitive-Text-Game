using Microsoft.AspNetCore.Routing;

namespace PrimitiveTextGame.Telegram.Modules.Common;

public interface IModule
{
	IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration);
	IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder, IConfiguration configuration);
}