using PrimitiveTextGame.StrategyPattern.Weapons.Knight;
using PrimitiveTextGame.Utilites;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class KnifeArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Knife armor";

        public override int ReduceDamage(IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Knife)))
            {
                return base.ReduceDamage(weapon);
            }
            return weapon.Damage;
        }
    }
}
