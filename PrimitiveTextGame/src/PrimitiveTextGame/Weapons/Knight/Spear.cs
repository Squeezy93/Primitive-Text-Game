using PrimitiveTextGame.Characters;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Knight
{
    public class Spear : IWeapon
    {
        public int Damage => 25;

        public string Name => "Spear";

        public void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
