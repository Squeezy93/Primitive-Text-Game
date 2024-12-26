using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions
{
    public interface ICharacterRepository : IRepository<Character, Guid>;    
}
