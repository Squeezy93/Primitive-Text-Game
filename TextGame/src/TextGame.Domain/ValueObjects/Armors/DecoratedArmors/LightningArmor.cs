using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class LightningArmor : ArmorDecorator
    {
        public LightningArmor(ArmorBase armor) : base(armor, "Lightning Armor")
        {
        }
    }
}
