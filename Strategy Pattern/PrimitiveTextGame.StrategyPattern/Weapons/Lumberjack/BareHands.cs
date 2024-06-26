using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Lumberjack
{
    public class BareHands : IWeapon
    {
        public int Damage => 15;

        public string Name => "Barehands";

        public void Attack(Character target)
        {
            target.TakeDamage(Damage, this);
        }
    }
}
