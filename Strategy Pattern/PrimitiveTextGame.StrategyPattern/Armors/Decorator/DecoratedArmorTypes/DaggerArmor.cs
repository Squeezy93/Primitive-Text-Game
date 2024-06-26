using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Mage;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class DaggerArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Dagger armor";

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Dagger)))
            {
                return base.ReduceDamage(damage, weapon);
            }
            return damage;
        }
    }
}
