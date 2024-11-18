namespace PrimitiveTextGame.Telegram.Modules.Common;

public interface IModule
{
	IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration);
}