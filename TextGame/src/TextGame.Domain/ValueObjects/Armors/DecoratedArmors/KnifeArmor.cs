using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class KnifeArmor : ArmorDecorator
    {
        public KnifeArmor(ArmorBase armor) : base(armor, "Knife Armor")
        {
        }
    }
}
