using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.Armors.ArmorsType
{
    public class LightArmor : BaseArmor
    {
        public LightArmor() : base(10)
        {
            Name = "Light armor";
        }        

        public override int ReduceDamage(IWeapon weapon)
        {
            return weapon.Damage - (weapon.Damage * Value / 100);            
        }
    }
}