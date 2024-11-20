using PrimitiveTextGame.Characters;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack
{
    public class BareHands : IWeapon
    {
        public int Damage => 15;

        public string Name => "Barehands";

        public void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
