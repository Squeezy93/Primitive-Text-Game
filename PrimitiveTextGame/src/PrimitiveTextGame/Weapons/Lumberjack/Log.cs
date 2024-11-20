using PrimitiveTextGame.Characters;
using PrimitiveTextGame.Weapons;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack
{
    public class Log : IWeapon
    {
        public int Damage => 20;

        public string Name => "Log";

        public void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
