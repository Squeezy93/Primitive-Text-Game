using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services
{
    public interface ITurnService
    {
        Task<bool> FirstTurn(Game game);
        Task<bool> NextTurn(string weaponName, long attackerId, long defenderId);
    }
}
