using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using Telegram.Bot.Types;

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

    //ef
    private User()
    {
        
    }
    public User(Update update, Character character)
    {
        Id = new Guid();
        UserTelegramId = update.CallbackQuery.From.Id;
        UserName = update.CallbackQuery.From.Username;
        Character = character;
        CharacterId = character.Id;
        Weapons = new List<Weapon>();
        Armors = new List<Armor>();
        Games = new List<Game>();
    }
}