using TextGame.Domain.ValueObjects.Armors.BaseArmors;

namespace TextGame.Domain.ValueObjects.Armors.DecoratedArmors
{
    public abstract class ArmorDecorator : ArmorBase
    {
        private ArmorBase _baseArmor;

        protected ArmorDecorator(ArmorBase armor, string name) : base(armor.Value, name)
        {
            _baseArmor = armor;
        }

        public ArmorBase GetBaseArmor() => _baseArmor;
    }
}
