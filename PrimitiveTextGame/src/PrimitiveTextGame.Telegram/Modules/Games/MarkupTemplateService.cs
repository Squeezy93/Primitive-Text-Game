using Microsoft.Extensions.Caching.Memory;
using PrimitiveTextGame.Telegram.Modules.Games.Constants;
using Scriban;
using Telegram.Bot.Types.ReplyMarkups;

namespace PrimitiveTextGame.Telegram.Modules.Games
{
    public class MarkupTemplateService : IMarkupTemplateService
    {
        private readonly IMemoryCache _memoryCache;

        public MarkupTemplateService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<InlineKeyboardMarkup> GetMarkupAsync(string templateName, Dictionary<string, object> parameters = default)
        {
            if (!_memoryCache.TryGetValue(TemplateConstants.InlineMarkupTemplate, out Dictionary<string, Dictionary<string, string>> inlineMarkupTemplate))
            {
                throw new InvalidOperationException("Parameters template not found in cache.");
            }

            if (!inlineMarkupTemplate.ContainsKey(templateName))
            {
                throw new InvalidOperationException($"Template '{templateName}' not found.");
            }

            var markupDictionary = inlineMarkupTemplate[templateName];

            var inlineKeyboardMarkup = new InlineKeyboardMarkup();

            foreach (var button in markupDictionary)
            {
                var templateMessage = Template.ParseLiquid(button.Value);
                var message = await templateMessage.RenderAsync(parameters);
                inlineKeyboardMarkup.AddButton(button.Key, message);
                inlineKeyboardMarkup.AddNewRow();
            }
            return inlineKeyboardMarkup;
        }
    }
}
