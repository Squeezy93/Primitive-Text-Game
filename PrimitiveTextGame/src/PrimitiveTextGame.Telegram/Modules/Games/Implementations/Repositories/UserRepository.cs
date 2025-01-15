using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Repositories;

public class UserRepository(ApplicationDataContext context) : Repository<User, Guid>(context), IUserRepository;