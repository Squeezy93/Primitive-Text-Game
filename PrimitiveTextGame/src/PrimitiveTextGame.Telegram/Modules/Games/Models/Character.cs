namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class Character : BaseEntity<Guid>
{
    public int Health { get; set; }
    public CharacterType CharacterType { get; set; }
    public string Name { get; set; }
}