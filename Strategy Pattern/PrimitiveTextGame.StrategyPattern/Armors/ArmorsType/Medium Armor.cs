using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors.ArmorsType
{
    public class MediumArmor : BaseArmor
    {
        public MediumArmor() : base(25)
        {
            Name = "Medium armor";
        }

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            return damage - (damage * Value / 100);
        }
    }
}