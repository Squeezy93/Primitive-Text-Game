namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services
{
    public interface IGameService
    {
        Task<bool> StartGame(long userId, long opponentId);
        Task<bool> HandleAttackCommand(string weaponName, long attackerId, long defenderId);
    }
}
