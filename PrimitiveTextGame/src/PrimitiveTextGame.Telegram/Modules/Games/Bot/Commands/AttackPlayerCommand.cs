using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class AttackPlayerCommand : ServiceScopeFactoryBase, IBotCommand
    {
        public AttackPlayerCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix => "attack";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.CallbackQuery.Message != null)
            {
                var originalKeyboard = update.CallbackQuery.Message.ReplyMarkup;
                if (originalKeyboard != null)
                {
                    await botClient.DeleteMessage(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
                }
            }
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;
            var ids = update.CallbackQuery.Data.Substring(Prefix.Length + 1).Split('_');
            var weaponName = ids[0];
            var attackerId = long.Parse(ids[1]);
            var defenderId = long.Parse(ids[2]);
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var turnService = scope.ServiceProvider.GetRequiredService<ITurnService>();
            await turnService.NextTurn(weaponName, attackerId, defenderId);
            return true;
        }
    }
}
