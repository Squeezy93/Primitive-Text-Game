using PrimitiveTextGame.StrategyPattern.Characters;

namespace PrimitiveTextGame.StrategyPattern.Weapons
{
    public interface IWeapon
    {
        int Damage { get; }
        string Name { get; }
        void Attack(Character target);
    }
}
