using Microsoft.Extensions.Caching.Memory;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class CreatePlayerCommand : ServiceScopeFactoryBase, IBotCommand
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<string, string> _messagesTemplateDictionary;

        public CreatePlayerCommand(IServiceScopeFactory serviceScopeFactory, IMemoryCache memoryCache) : base(serviceScopeFactory)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            if (_memoryCache.TryGetValue(TemplateConstants.MessagesTemplate, out var messagesTemplate))
            {
                var value = messagesTemplate as Dictionary<string, string>;
                if (value != null)
                {
                    _messagesTemplateDictionary = value;
                }
                //TODO: надо подумать на случай если нет в кэше что делать
            }
        }

        public string Prefix { get; } = "create_player";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var parametersTemplateService = scope.ServiceProvider.GetRequiredService<IParametersTemplateService>();
            var markupTemplateService = scope.ServiceProvider.GetRequiredService<IMarkupTemplateService>();
            var user = await userService.CreateNewUser(update);
            var template = _messagesTemplateDictionary[TemplateConstants.UserCreated];
            var parameters = await parametersTemplateService.GetParametersAsync(TemplateConstants.UserCreated, user);

            var markup = await markupTemplateService.GetMarkupAsync(TemplateConstants.UserCreated);
            await notificationService.SendNotification(user.UserTelegramId, template, parameters, markup);            
            return true;
        }
    }
}
