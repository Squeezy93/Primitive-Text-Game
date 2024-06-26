using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Mage;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class FireArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Fire armor";

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Fire)))
            {
                return base.ReduceDamage(damage, weapon);
            }
            return damage;
        }
    }
}