using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors.ArmorsType
{
    public class HeavyArmor : BaseArmor
    {
        public HeavyArmor() : base(50)
        {
            Name = "Heavy armor";            
        }

        public override int ReduceDamage(IWeapon weapon)
        {
            return weapon.Damage - (weapon.Damage * Value / 100);
        }
    }
}