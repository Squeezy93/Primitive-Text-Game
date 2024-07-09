using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors.ArmorsType
{
    public class MediumArmor : BaseArmor
    {
        public MediumArmor() : base(25)
        {
            Name = "Medium armor";
        }

        public override int ReduceDamage(IWeapon weapon)
        {
            return weapon.Damage - (weapon.Damage * Value / 100);
        }
    }
}