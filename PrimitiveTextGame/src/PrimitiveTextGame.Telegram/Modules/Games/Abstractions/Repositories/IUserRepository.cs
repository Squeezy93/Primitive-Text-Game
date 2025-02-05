using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    void EndGameUpdate(User user); 
}