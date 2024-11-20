using PrimitiveTextGame.Telegram.Modules.Game.Models.Armors;
using PrimitiveTextGame.Telegram.Modules.Game.Models.Weapons;

namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Characters;

public abstract class Character
{
    public Guid Id { get; protected set; }
    public int Health { get; protected set; }
    public string CharacterClass { get; protected set; }
    public IWeapon? Weapon { get; protected set; }
    public List<IArmor> Armors { get; protected set; } = new();

    public Character(Guid Id, string characterClass, int health = 100)
    {
        this.Id = Id;
        Health = health;
        CharacterClass = characterClass;
    }

    public void TakeDamage(IWeapon weapon)
    {
        Health -= weapon.Damage;
    }

    public void Attack(Character target)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }
        Weapon?.Attack(target);
    }

    public void SetWeapon(IWeapon weapon)
    {
        if (weapon == null)
        {
            throw new ArgumentNullException(nameof(weapon));
        }
        Weapon = weapon;        
    }    
}