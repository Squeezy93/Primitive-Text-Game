using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class Character : BaseEntity<Guid>
{
    public int Health { get; set; }
    public CharacterType CharacterType { get; set; }
    public string Name { get; set; }

    public Character(CharacterType type, string name)
    {
        Health = 100;
        CharacterType = type;
        Name = name;
    }
    
    //ef
    private Character()
    {
        
    }    
}