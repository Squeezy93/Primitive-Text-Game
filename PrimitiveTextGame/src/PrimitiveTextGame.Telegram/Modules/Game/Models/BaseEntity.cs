namespace PrimitiveTextGame.Telegram.Modules.Game.Models;

public class BaseEntity<T>
{
	public T Id { get; set; }
	public DateTime CreateDate { get; set; }
	public DateTime DeleteDate { get; set; }
}