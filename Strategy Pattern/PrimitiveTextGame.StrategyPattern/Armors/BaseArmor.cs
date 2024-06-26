using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors
{
    public abstract class BaseArmor(int value)
    {
        public int Value { get; protected set; } = value;
        public virtual string Name { get; protected set; }       

        public abstract int ReduceDamage(int damage, IWeapon weapon);
    }
}