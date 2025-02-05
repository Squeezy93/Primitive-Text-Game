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
        private readonly Dictionary<string, string> _templateDictionary;

        public CreatePlayerCommand(IServiceScopeFactory serviceScopeFactory, IMemoryCache memoryCache) : base(serviceScopeFactory)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            if (_memoryCache.TryGetValue(TemplateConstants.Templates, out var templates))
            {
                var value = templates as Dictionary<string, string>;
                if (value != null)
                {
                    _templateDictionary = value;
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
            var user = await userService.CreateNewUser(update);
            var parameters = new { userName = user.UserName, characterName = user.Character.Name };

            var markup = new InlineKeyboardMarkup()
                .AddButton("Найти соперника", "search")
                .AddNewRow()
                .AddButton("Поменять персонажа", "choose_player_character")
                .AddNewRow()
                .AddButton("Покинуть игру", "quit_game");
            await notificationService.SendNotification(user.UserTelegramId, _templateDictionary[TemplateConstants.UserCreated], parameters, markup);
            
            return true;
        }
    }
}
