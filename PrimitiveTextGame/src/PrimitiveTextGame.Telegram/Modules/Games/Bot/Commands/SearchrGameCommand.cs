using Microsoft.Extensions.Caching.Memory;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class SearchrGameCommand : ServiceScopeFactoryBase, IBotCommand
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<string, string> _messagesTemplateDictionary;
        public SearchrGameCommand(IServiceScopeFactory serviceScopeFactory, IMemoryCache memoryCache) : base(serviceScopeFactory)
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

        public string Prefix => "search";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var parametersTemplateService = scope.ServiceProvider.GetRequiredService<IParametersTemplateService>();
            var markupTemplateService = scope.ServiceProvider.GetRequiredService<IMarkupTemplateService>();
            var user = await userService.StartSearchingForGame(update.CallbackQuery.From.Id);
            var opponent = await userService.FindOpponent(user);
            if (opponent is not null)
            {
                var userTemplate = _messagesTemplateDictionary[TemplateConstants.GameInvitationForUser];
                var opponentTemplate = _messagesTemplateDictionary[TemplateConstants.GameInvitationForOpponent];
                var parameters = await parametersTemplateService.GetParametersAsync(TemplateConstants.GameInvitationForUser, user,opponent);
                var markupForUser = await markupTemplateService.GetMarkupAsync(TemplateConstants.GameInvitationForUser, parameters);
                var markupForOpponent = await markupTemplateService.GetMarkupAsync(TemplateConstants.GameInvitationForOpponent, parameters);
                await notificationService.SendNotification(user.UserTelegramId, userTemplate, parameters, markupForUser);
                await notificationService.SendNotification(opponent.UserTelegramId, opponentTemplate, parameters, markupForOpponent);
            }
            else
            {
                var template = _messagesTemplateDictionary[TemplateConstants.StartSearching];
                var parameters = await parametersTemplateService.GetParametersAsync(TemplateConstants.StartSearching, user);
                var inlineMarkup = await markupTemplateService.GetMarkupAsync(TemplateConstants.StartSearching, parameters);
                await notificationService.SendNotification(user.UserTelegramId, template, markup: inlineMarkup);
            }
            return true;
        }
    }
}
