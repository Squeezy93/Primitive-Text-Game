using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using Telegram.Bot.Types;

namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class Character : BaseEntity<Guid>
{
    public int Health { get; set; }
    public CharacterType CharacterType { get; set; }
    public string Name { get; set; }

    public Character(Update update)
    {
        Id = Guid.NewGuid();
        Health = 100;
        CharacterType = GetCharacterType(update);
        Name = CharacterType.ToString();
    }
    //ef
    private Character()
    {
        
    }
    private CharacterType GetCharacterType(Update update)
    {
        var parts = update.CallbackQuery.Data.Split('_');
        var characterTypeMap = new Dictionary<string, CharacterType>
        {
            { "mage", CharacterType.Mage },
            { "knight", CharacterType.Knight },
            { "lumberjack", CharacterType.Lumberjack }
        };

        foreach (var part in parts)
        {
            if (characterTypeMap.TryGetValue(part, out var characterType))
            {
                return characterType;
            }
        }
        return CharacterType.Knight;
    }
}