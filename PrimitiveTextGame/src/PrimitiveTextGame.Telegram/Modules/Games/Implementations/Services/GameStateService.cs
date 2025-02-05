using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services;
using PrimitiveTextGame.Telegram.Modules.Games.Models;
using System.Collections.Concurrent;

namespace PrimitiveTextGame.Telegram.Modules.Games.Services
{
    public class GameStateService : IGameStateService
    {
        private readonly ConcurrentDictionary<long, Game> _games = new();

        public void AddGame(long telegramUserId, Game game)
        {
            _games.TryAdd(telegramUserId, game);
        }

        public Game GetGame(long telegramUserId)
        {
            return _games.TryGetValue(telegramUserId, out var game) ? game : null;
        }

        public void RemoveGame(long telegramUserId)
        {
            _games.TryRemove(telegramUserId, out _);
        }
    }
}
