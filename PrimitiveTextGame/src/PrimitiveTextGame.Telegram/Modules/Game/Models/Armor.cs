namespace PrimitiveTextGame.Telegram.Modules.Game.Models;

public class Armor : BaseEntity<Guid>
{
	public ArmorLevel ArmorLevel { get; set; }
	public string Name { get; set; }
	public ArmorType ArmorType { get; set; }
	public List<User> Users { get; set; }
}