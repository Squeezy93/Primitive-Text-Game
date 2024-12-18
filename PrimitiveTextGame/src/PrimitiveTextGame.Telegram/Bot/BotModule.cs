using PrimitiveTextGame.Telegram.Modules.Common;
using Telegram.Bot;

namespace PrimitiveTextGame.Telegram.Bot
{
    public class BotModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<BotProcess>();
            services.AddSingleton<ITelegramBotClient>(provider =>
            {
                var token = configuration.GetSection("TelegramBot")["Token"];
                return new TelegramBotClient(token);
            });

            return services;
        }
    }
}
