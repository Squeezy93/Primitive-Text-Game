namespace TextGame.Domain.ValueObjects.Armors.BaseArmors
{
    public abstract class ArmorBase
    {
        public int Value { get; protected set; }
        public string Name { get; protected set; }

        protected ArmorBase(int value, string name)
        {
            Value = value;
            Name = name;
        }
    }
}
