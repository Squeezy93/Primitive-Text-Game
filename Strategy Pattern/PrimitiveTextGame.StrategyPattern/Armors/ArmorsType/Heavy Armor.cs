using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors.ArmorsType
{
    public class HeavyArmor : BaseArmor
    {
        public HeavyArmor() : base(50)
        {
            Name = "Heavy armor";            
        }

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            return damage - (damage * Value / 100);
        }
    }
}