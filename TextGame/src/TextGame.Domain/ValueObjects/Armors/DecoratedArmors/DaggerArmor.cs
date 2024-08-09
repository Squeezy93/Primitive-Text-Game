using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class DaggerArmor : ArmorDecorator
    {
        public DaggerArmor(ArmorBase armor) : base(armor, "Dagger Armor")
        {
        }
    }
}
