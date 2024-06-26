using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors.ArmorsType
{
    public class LightArmor : BaseArmor
    {
        public LightArmor() : base(10)
        {
            Name = "Light armor";
        }        

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            return damage - (damage * Value / 100);            
        }
    }
}