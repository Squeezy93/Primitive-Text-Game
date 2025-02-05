using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services
{
    public interface INotificationService
    {
        Task SendUserCreated(User user);
        Task SendGameInvitation(User user, User opponent);
        Task SendReturnMessage(User user);
        Task SendFirstTurn(User firstPlayer, User secondPlayer);
        Task SendNextTurn(User attacker, User defender, Weapon weapon);
        Task SendEndgame(User attacker, User defender);
        Task SendWaitingForOpponent(long userTelegramId);
        Task SendNewCharacterSelection(long userTelegramId);
        Task SendChangeCharacterSelection(User user);
        Task SendChangedCharacter(User user);
        Task SendStartSearching(long userTelegramId);
        Task SendStopSearching(long userTelegramId);
        Task SendDeclineGame(long userTelegramId);
        Task SendReturnToSearch(long userTelegramId);
    }
}
