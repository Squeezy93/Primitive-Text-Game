using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.WeaponSpecifications
{
    public class GetByCharacterTypeSpecification : SpecificationBase<Weapon>
    {
        public GetByCharacterTypeSpecification(CharacterType type)
        {
            Criteria = weapon => weapon.CharacterType == type;
        }
    }
}
