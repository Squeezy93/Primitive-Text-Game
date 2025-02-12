using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games
{
    public interface IMarkupTemplateService
    {
        Task<InlineKeyboardMarkup> GetMarkupAsync(string templateName, Dictionary<string, object> parameters = default);
    }
}
