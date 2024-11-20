namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors
{
    public interface IArmor
    {
        Guid Id { get; }
        int Value { get; } 
        string Name { get; }
    }
}
