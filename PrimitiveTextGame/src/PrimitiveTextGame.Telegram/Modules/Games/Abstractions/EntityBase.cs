namespace PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

public class EntityBase<TId>
    where TId : IEquatable<TId>
{
    public EntityBase()
    {
        CreateDate = DateTime.UtcNow;
    }

    public TId Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
}