using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Knight;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class SwordArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Sword armor";

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Sword)))
            {
                return base.ReduceDamage(damage, weapon);
            }
            return damage;
        }
    }
}
