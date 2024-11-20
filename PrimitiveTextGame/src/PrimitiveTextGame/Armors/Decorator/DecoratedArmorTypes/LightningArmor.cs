using PrimitiveTextGame.StrategyPattern.Weapons.Mage;
using PrimitiveTextGame.Utilites;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class LightningArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Lightning armor";

        public override int ReduceDamage(IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Lightning)))
            {
                return base.ReduceDamage(weapon);
            }
            return weapon.Damage;
        }
    }
}
