using PrimitiveTextGame.Armors;
using PrimitiveTextGame.StrategyPattern.Weapons;
using System.Text.Json.Serialization;

namespace PrimitiveTextGame.Characters
{
    public abstract class Character
    {
        public int Health { get; protected set; } = 100;
        public string Name { get; protected set; }
        public IWeapon Weapon { get; private set; }
        public List<BaseArmor> Armors { get; set; } = new List<BaseArmor>();

        public void TakeDamage(int damage, IWeapon weapon)
        {
            int reducedDamage = CalculateReduceDamage(damage, weapon);
            Health -= reducedDamage;
        }

        [JsonConstructor]
        public Character()
        {

        }
        protected Character(string name)
        {
            Name = name;
        }

        public void Attack(Character character) => Weapon.Attack(character);

        private int CalculateReduceDamage(int damage, IWeapon weapon)
        {
            int totalReducedDamage = damage;

            foreach (var armor in Armors)
            {
                totalReducedDamage = armor.ReduceDamage(totalReducedDamage, weapon);
            }

            return totalReducedDamage;
        }

        public IWeapon SetWeapon(IWeapon weapon) => Weapon = weapon;        
    }
}