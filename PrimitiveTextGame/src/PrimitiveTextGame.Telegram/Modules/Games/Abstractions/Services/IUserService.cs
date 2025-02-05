using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Services
{
    public interface IUserService
    {
        Task<Models.User> CreateNewUser(Update update);
        Task<Models.User> StartSearchingForGame(long userTelegramId);
        Task<Models.User> FindOpponent(Models.User currentUser);
    }
}
