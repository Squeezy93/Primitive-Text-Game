using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class Weapon : EntityBase<Guid>
{
    public Weapon(string name, CharacterType characterType, int damage)
    {
        Name = name;
        CharacterType = characterType;
        Damage = damage;
    }
    public string Name { get; set; }
    public CharacterType CharacterType { get; set; }
    public int Damage { get; set; }
    public List<User> Users { get; set; }
}