using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Data;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Repositories
{
    public class CharacterRepository(ApplicationDataContext context) : Repository<Character, Guid>(context), ICharacterRepository;
}
