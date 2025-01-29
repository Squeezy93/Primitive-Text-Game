using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;

public interface IGameRepository : IRepository<Game, Guid>;
