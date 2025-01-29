using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services
{
    public interface IGameStateService
    {
        void AddGame(long telegramUserId, Game game);
        Game GetGame(long telegramUserId);
        void RemoveGame(long telegramUserId);
    }
}
