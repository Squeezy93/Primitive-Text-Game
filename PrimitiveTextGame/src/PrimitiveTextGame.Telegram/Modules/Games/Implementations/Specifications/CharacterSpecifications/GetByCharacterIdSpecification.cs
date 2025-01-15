using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.CharacterSpecifications
{
    public class GetByCharacterIdSpecification : SpecificationBase<Character>
    {
        public GetByCharacterIdSpecification(Guid id)
        {
            Criteria = character => character.Id == id;
        }
    }
}
