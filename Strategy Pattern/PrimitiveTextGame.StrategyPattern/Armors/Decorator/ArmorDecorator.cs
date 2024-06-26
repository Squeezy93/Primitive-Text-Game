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

        public override int ReduceDamage(int damage, IWeapon weapon)
        {
            if (BaseArmor == null)
            {
                throw new NullReferenceException(nameof(BaseArmor));
            }
            return BaseArmor.ReduceDamage(damage, weapon);
        }

        public BaseArmor SetArmor(BaseArmor armor)
        {
            BaseArmor = armor;
            Value = armor.Value;
            return this;
        }

        public string GetBaseName() => BaseArmor.Name;
    }
}