namespace PrimitiveTextGame.Telegram.Modules.Game.Models;

public class Weapon : BaseEntity<Guid>
{
	public string Name { get; set; }
	public CharacterType CharacterType { get; set; }
	public int Damage { get; set; }
	public List<User> Users { get; set; }
}