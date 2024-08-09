using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public class LogArmor : ArmorDecorator
    {
        public LogArmor(ArmorBase armor) : base(armor, "Log Armor")
        {
        }
    }
}
