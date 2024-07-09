using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors
{
    public abstract class BaseArmor
    {
        public int Value { get; protected set; }
  
        public virtual string Name { get; protected set; }

        public BaseArmor(int value)
        {
            Value = value;
        }
        public abstract int ReduceDamage(IWeapon weapon);
    }
}