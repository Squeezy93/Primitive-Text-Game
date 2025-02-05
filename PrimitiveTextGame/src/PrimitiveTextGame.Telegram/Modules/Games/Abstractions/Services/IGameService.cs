using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services
{
    public interface IGameService
    {
        Task<bool> StartGame(long userId, long opponentId);
        Task<bool> EndGame(User attacker, User defender);
    }
}
