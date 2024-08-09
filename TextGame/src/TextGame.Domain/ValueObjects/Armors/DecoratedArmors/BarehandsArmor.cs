using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class BarehandsArmor : ArmorDecorator
    {
        public BarehandsArmor(ArmorBase armor) : base(armor, "Barehands Armor")
        {
        }
    }
}
