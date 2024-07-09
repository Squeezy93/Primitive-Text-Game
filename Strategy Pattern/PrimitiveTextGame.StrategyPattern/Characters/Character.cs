using PrimitiveTextGame.Armors;
using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Characters
{
    public abstract class Character
    {
        public int Health { get; protected set; } = 100;      
        public string Name { get; protected set; }        
        public IWeapon Weapon { get; private set; }       
        public List<BaseArmor> Armors { get; set; } = new List<BaseArmor>();

        public void TakeDamage(IWeapon weapon)
        {
            int reducedDamage = CalculateReduceDamage(weapon);
            Health -= reducedDamage;
        }
        
        public Character()
        {

        }

        protected Character(string name)
        {
            Name = name;
        }

        public void Attack(Character character) => Weapon.Attack(character);

        private int CalculateReduceDamage(IWeapon weapon)
        {
            int totalReducedDamage = weapon.Damage;

            foreach (var armor in Armors)
            {
                totalReducedDamage = armor.ReduceDamage(weapon);
            }

            return totalReducedDamage;
        }

        public IWeapon SetWeapon(IWeapon weapon) => Weapon = weapon;

        public void SetHealth(int health) => Health = health;
    }
}