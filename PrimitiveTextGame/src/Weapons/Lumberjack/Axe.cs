using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack
{
    public class Axe : IWeapon
    {
        public int Damage => 30;

        public string Name => "Axe";

        public void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
