using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.CharacterSpecifications
{
    public class GetByCharacterNameSpecification : SpecificationBase<Character>
    {
        public GetByCharacterNameSpecification(string name)
        {
            Criteria = character => character.Name.ToLower() == name.ToLower();
        }
    }
}
