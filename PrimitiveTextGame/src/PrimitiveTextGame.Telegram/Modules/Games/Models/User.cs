namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class User : BaseEntity<Guid>
{
    public long UserTelegramId { get; set; }
    public string UserName { get; set; }
    public Character Character { get; set; }
    public Guid CharacterId { get; set; }
    public List<Weapon> Weapons { get; set; }
    public List<Armor> Armors { get; set; }
    public List<Game> Games { get; set; }
}