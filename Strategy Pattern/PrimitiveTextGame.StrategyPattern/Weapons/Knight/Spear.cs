using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Knight
{
    public class Spear : IWeapon
    {
        public int Damage => 25;

        public string Name => "Spear";

        public void Attack(Character target)
        {
            target.TakeDamage(Damage, this);
        }
    }
}
