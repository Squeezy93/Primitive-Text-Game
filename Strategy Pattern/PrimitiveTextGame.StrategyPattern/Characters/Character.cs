using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.StrategyPattern.Characters
{
    public abstract class Character(string name)
    {
        public int Health { get; protected set; } = 100;
        public string Name { get; protected set; } = name;        
        public IWeapon Weapon { get; set; }

        public void TakeDamage(int damage) => Health -= damage;

        public void Attack(Character character) => Weapon.Attack(character);
    }
}