using PrimitiveTextGame.StrategyPattern.Weapons;
using PrimitiveTextGame.StrategyPattern.Weapons.Knight;
using PrimitiveTextGame.Utilites;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class KnifeArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Knife armor";

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(Knife)))
            {
                return base.ReduceDamage(damage, weapon);
            }
            return damage;
        }
    }
}
