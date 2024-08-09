using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Knight
{
    public class Knife : IWeapon
    {
        public int Damage => 20;

        public string Name => "Knife";

        public void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
