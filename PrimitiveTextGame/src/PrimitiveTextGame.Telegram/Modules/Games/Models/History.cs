namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class History : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid WeaponId { get; set; }
    public Weapon Weapon { get; set; }
    public Guid GameId { get; set; }
    public Game Game { get; set; }
    public int Damage { get; set; }
    public int Order { get; set; }
    public int Health { get; set; }
}