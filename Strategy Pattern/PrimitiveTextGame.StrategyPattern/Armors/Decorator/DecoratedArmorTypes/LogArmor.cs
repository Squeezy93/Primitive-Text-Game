using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class LogArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Log armor";

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Log)))
            {
                return base.ReduceDamage(damage, weapon);
            }
            return damage;
        }
    }
}
