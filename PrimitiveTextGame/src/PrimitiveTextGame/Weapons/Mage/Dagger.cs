using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Mage
{
    public class Dagger : IWeapon
    {
        public int Damage => 15;

        public string Name => "Dagger";

        public void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
