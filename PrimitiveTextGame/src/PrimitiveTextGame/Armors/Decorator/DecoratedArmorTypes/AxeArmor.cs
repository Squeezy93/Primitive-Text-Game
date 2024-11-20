using PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack;
using PrimitiveTextGame.Utilites;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class AxeArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Axe armor";

        public override int ReduceDamage(IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Axe)))
            {
                return base.ReduceDamage(weapon);
            }
            return weapon.Damage;
        }
    }
}
