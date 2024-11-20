
namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Characters.CharacterClasses
{
    public class Knight : Character
    {
        public Knight(Guid Id, int health = 100) : base(Id, "Knight", health)
        {
        }
    }
}
