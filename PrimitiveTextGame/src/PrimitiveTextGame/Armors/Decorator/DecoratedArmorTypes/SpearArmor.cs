using PrimitiveTextGame.StrategyPattern.Weapons.Knight;
using PrimitiveTextGame.Utilites;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class SpearArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Spear armor";

        public override int ReduceDamage(IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Spear)))
            {
                return base.ReduceDamage(weapon);
            }
            return weapon.Damage;
        }
    }
}
