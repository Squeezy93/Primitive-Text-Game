using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications.ArmorSpecifications
{
    public class GetByArmorLevelSpecification : SpecificationBase<Armor>
    {
        public GetByArmorLevelSpecification(ArmorLevel armorLevel)
        {
            Criteria = armor => armor.ArmorLevel == armorLevel;
        }
    }
}
