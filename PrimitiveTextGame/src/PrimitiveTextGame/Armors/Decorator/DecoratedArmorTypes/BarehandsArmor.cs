using PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack;
using PrimitiveTextGame.Utilites;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.Armors.Decorator.DecoratedArmorTypes
{
    public class BarehandsArmor(BaseArmor armor) : ArmorDecorator(armor)
    {
        public override string Name => "Barehands armor";

        public override int ReduceDamage(IWeapon weapon)
        {
            if (WeaponHelper.IsWeaponType(weapon, typeof(BareHands)))
            {
                return base.ReduceDamage(weapon);
            }
            return weapon.Damage;
        }
    }
}
