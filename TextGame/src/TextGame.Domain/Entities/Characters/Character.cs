using TextGame.Domain.ValueObjects.Armors.DecoratedArmors;
using TextGame.Domain.ValueObjects.Weapons;

namespace TextGame.Domain.Entities.Characters
{
    public abstract class Character
    {
        public string PlayerName { get; private set; }
        public string ClassName { get; private set; }
        public int Health { get; private set; }
        public IWeapon Weapon { get; set; }
        public List<ArmorDecorator> Armors { get; private set; } = new();

        public Character(string nickName, int health, string className)
        {
            PlayerName = nickName;
            Health = health;
            ClassName = className;
        }

        public void TakeDamage(IWeapon weapon)
        {
            Health -= weapon.Damage;
        }

        public void Attack(Character character) => Weapon.Attack(character);
    }
}
