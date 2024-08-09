using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class SwordArmor : ArmorDecorator
    {
        public SwordArmor(ArmorBase armor) : base(armor, "Sword Armor")
        {
        }
    }
}
