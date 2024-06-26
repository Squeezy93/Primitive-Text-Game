using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack
{
    public class Log : IWeapon
    {
        public int Damage => 20;

        public string Name => "Log";

        public void Attack(Character target)
        {
            target.TakeDamage(Damage, this);
        }
    }
}
