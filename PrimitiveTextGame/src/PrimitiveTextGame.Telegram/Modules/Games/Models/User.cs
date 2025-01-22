using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class User : EntityBase<Guid>
{
    public bool IsSearchingForGame { get; set; }
    public bool IsPlayingGame { get; set; }
    public long UserTelegramId { get; set; }
    public string UserName { get; set; }
    public Character Character { get; set; }
    public Guid CharacterId { get; set; }
    public int Health { get; set; }
    public List<Weapon> Weapons { get; set; }
    public List<Armor> Armors { get; set; }
    public List<Game> Games { get; set; }

    //ef
    private User()
    {

    }
    public User(long userTelegramId, string userName, Character character, List<Weapon> weapons, List<Armor> armors, List<Game> games = null, int health = 100)
    {
        Id = Guid.NewGuid();
        UserTelegramId = userTelegramId;
        UserName = userName;
        IsSearchingForGame = false;
        IsPlayingGame = false;
        Character = character;
        CharacterId = character.Id;
        Health = health;
        Weapons = weapons ?? new List<Weapon>();
        Armors = armors ?? new List<Armor>();
        Games = games ?? new List<Game>();
    }    
}