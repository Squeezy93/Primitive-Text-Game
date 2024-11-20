using PrimitiveTextGame.Characters;

namespace PrimitiveTextGame.Weapons
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
