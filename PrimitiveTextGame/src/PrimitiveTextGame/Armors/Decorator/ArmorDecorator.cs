using PrimitiveTextGame.StrategyPattern.Weapons;

namespace PrimitiveTextGame.Armors.Decorator
{
    public abstract class ArmorDecorator : BaseArmor
    {
        protected BaseArmor BaseArmor { get; private set; }

        public ArmorDecorator(BaseArmor armor) : base(armor.Value)
        {
            BaseArmor = armor;
        }

        public override int ReduceDamage(IWeapon weapon)
        {
            if (BaseArmor == null)
            {
                throw new NullReferenceException(nameof(BaseArmor));
            }
            return BaseArmor.ReduceDamage(weapon);
        }

        public BaseArmor SetArmor(BaseArmor armor)
        {
            BaseArmor = armor;
            Value = armor.Value;
            return this;
        }

        public BaseArmor GetBaseArmor() => BaseArmor;
    }
}