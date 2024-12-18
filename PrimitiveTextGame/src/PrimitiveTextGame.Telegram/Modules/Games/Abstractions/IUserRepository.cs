using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

public interface IUserRepository : IRepository<User, Guid>;