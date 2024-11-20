using PrimitiveTextGame.StrategyPattern.Weapons.Mage;
using PrimitiveTextGame.Utilites;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class FireArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Fire armor";

        public override int ReduceDamage(IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Fire)))
            {
                return base.ReduceDamage(weapon);
            }
            return weapon.Damage;
        }
    }
}