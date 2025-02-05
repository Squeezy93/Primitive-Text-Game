using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Repositories;

public class UserRepository(ApplicationDataContext context) : Repository<User, Guid>(context), IUserRepository
{
    public void EndGameUpdate(User user)
    {
        user.Armors.Clear();
        Update(user);
    }
}