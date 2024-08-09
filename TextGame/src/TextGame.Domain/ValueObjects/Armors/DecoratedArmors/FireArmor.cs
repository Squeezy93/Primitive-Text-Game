using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class FireArmor : ArmorDecorator
    {
        public FireArmor(ArmorBase armor) : base(armor, "Fire Armor")
        {
        }
    }
}
