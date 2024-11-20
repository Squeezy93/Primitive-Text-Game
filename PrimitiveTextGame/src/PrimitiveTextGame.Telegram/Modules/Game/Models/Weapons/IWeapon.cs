using PrimitiveTextGame.Telegram.Modules.Game.Models.Characters;

namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Weapons
{
    public interface IWeapon
    {
        int Damage { get; }
        string Name { get; }
        void Attack(Character target)
        {
            target.TakeDamage(this);
        }
    }
}
