using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class Character : EntityBase<Guid>
{
    public CharacterType CharacterType { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; }

    public Character(CharacterType type, string name)
    {
        CharacterType = type;
        Name = name;
    }
    //ef
    private Character()
    {

    }
}