using PrimitiveTextGame.StrategyPattern.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Mage
{
    public class Lightning : IWeapon
    {
        public int Damage => 25;
        public string Name => "Lightning";

        public void Attack(Character target)
        {
            target.TakeDamage(Damage);
        }
    }
}
