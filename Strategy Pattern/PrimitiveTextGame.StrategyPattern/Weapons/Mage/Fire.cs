using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons.Mage
{
    public class Fire : IWeapon
    {
        public int Damage => 20;
        public string Name => "Fire";

        public void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
