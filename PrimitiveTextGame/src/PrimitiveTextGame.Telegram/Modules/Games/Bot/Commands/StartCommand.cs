using Microsoft.Extensions.Caching.Memory;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.UserSpecifications;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands;

public class StartCommand : ServiceScopeFactoryBase, IBotCommand
{
    private readonly IMemoryCache _memoryCache;
    private readonly Dictionary<string, string> _messagesTemplateDictionary;
    public StartCommand(IServiceScopeFactory serviceScopeFactory, IMemoryCache memoryCache) : base(serviceScopeFactory)
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
    public string Prefix { get; } = "start";

    public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
    {
        if (update.Message == null || update.Message.Chat == null) return false;
        using var scope = ServiceScopeFactory.CreateAsyncScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var parametersTemplateService = scope.ServiceProvider.GetRequiredService<IParametersTemplateService>();
        var markupTemplateService = scope.ServiceProvider.GetRequiredService<IMarkupTemplateService>();
        bool isUserExists = await userRepository.IsExists(new GetByUserTelegramIdSpecification(update.Message.From.Id));
        var user = await userRepository.GetAsync(new GetByUserTelegramIdSpecification(update.Message.From.Id));
        if (isUserExists)
        {
            var template = _messagesTemplateDictionary[TemplateConstants.UserReturned];
            var parameters = await parametersTemplateService.GetParametersAsync(TemplateConstants.UserReturned, user);
            var inlineMarkup = await markupTemplateService.GetMarkupAsync(TemplateConstants.UserReturned);
            await notificationService.SendNotification(user.UserTelegramId, template, parameters, inlineMarkup);
        }
        else
        {
            var template = _messagesTemplateDictionary[TemplateConstants.CreateNewUser];
            var inlineMarkup = await markupTemplateService.GetMarkupAsync(TemplateConstants.CreateNewUser);
            await notificationService.SendNotification(user.UserTelegramId, template, markup: inlineMarkup);
        }
        return true;
    }
}