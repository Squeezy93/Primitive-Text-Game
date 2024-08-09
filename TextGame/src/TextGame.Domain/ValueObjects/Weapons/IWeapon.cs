using TextGame.Domain.Entities.Characters;

namespace TextGame.Domain.ValueObjects.Weapons
{
    public interface IWeapon
    {
        int Damage { get; }
        string Name { get; }
        void Attack(Character character);
    }
}
