namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

public class BaseEntity<TId>
    where TId : IEquatable<TId>
{
    public BaseEntity()
    {
        CreateDate = DateTime.UtcNow;
    }

    public TId Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
}