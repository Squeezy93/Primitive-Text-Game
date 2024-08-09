using TextGame.Domain.Entities.Characters;

namespace TextGame.Domain.ValueObjects.Weapons
{
    public abstract class WeaponBase : IWeapon
    {
        public abstract int Damage { get; }
        public abstract string Name { get; }

        public void Attack(Character character)
        {
            character.TakeDamage(this);
        }
    }
}
