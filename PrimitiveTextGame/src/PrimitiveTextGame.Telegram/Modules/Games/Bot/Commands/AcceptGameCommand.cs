﻿using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PrimitiveTextGame.Telegram.Modules.Games.Bot.Commands
{
    public class AcceptGameCommand : ServiceScopeFactoryBase, IBotCommand
    {
        
        public AcceptGameCommand(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public string Prefix => "accept_game";

        public async Task<bool> ExecuteAsync(ITelegramBotClient botClient, Update update)
        {
            if (update.Type != UpdateType.CallbackQuery || update.CallbackQuery.Data == null) return false;
            if (!update.CallbackQuery.Data.StartsWith(Prefix)) return false;

            var ids = update.CallbackQuery.Data.Substring(Prefix.Length + 1).Split('_');
            var userId = long.Parse(ids[0]);
            var opponentId = long.Parse(ids[1]);
            using var scope = ServiceScopeFactory.CreateAsyncScope();
            var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
            await gameService.StartGame(userId, opponentId);
            return true;
        }
    }
}
