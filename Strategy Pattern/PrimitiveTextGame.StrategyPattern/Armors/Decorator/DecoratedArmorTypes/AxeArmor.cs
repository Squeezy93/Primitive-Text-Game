using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class AxeArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Axe armor";

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Axe)))
            {
                return base.ReduceDamage(damage, weapon);
            }
            return damage;
        }
    }
}
