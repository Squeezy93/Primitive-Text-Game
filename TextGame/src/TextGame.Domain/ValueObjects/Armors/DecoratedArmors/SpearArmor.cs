using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class SpearArmor : ArmorDecorator
    {
        public SpearArmor(ArmorBase armor) : base(armor, "Spear Armor")
        {
        }
    }
}
