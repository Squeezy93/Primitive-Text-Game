using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Mage;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class LightningArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Lightning armor";

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Lightning)))
            {
                return base.ReduceDamage(damage, weapon);
            }
            return damage;
        }
    }
}
