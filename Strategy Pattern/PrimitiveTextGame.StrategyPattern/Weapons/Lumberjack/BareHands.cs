using PrimitiveTextGame.StrategyPattern.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack
{
    public class BareHands : IWeapon
    {
        public int Damage => 15;

        public string Name => "BareHands";

        public void Attack(Character target)
        {
            target.TakeDamage(Damage);
        }
    }
}
