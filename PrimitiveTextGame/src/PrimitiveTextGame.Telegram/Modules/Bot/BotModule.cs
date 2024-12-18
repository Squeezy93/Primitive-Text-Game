using PrimitiveTextGame.Telegram.Modules.Common;
using Telegram.Bot;

namespace PrimitiveTextGame.Telegram.Modules.Bot
{
    public class BotModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("Primitive_text_game_bot")
                .RemoveAllLoggers()
                .AddTypedClient<ITelegramBotClient>((httpClient) =>
            {
                TelegramBotClientOptions options =
                    new TelegramBotClientOptions(configuration.GetSection("TelegramBot")["Token"]);
                return new TelegramBotClient(options, httpClient);
            });
            
            services.AddHostedService<BotProcess>();

            return services;
        }
    }
}
