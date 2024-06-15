using PrimitiveTextGame.StrategyPattern.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Knight
{
    public class Sword : IWeapon
    {
        public int Damage => 30;

        public string Name => "Sword";

        public void Attack(Character target)
        {
            target.TakeDamage(Damage);
        }
    }
}