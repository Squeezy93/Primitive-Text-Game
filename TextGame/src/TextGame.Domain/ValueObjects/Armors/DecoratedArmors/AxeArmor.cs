using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class AxeArmor : ArmorDecorator
    {
        public AxeArmor(ArmorBase armor) : base(armor, "Axe Armor")
        {
        }
    }
}
