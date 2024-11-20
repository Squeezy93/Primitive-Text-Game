namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors.ArmorTypes
{
    public class AxeArmor : IArmor
    {
        public Guid Id { get; set; }
        public int Value { get; private set; }
        public string Name { get; private set; } = "Axe Armor";
        public AxeArmor(ArmorLevels armorLevel = ArmorLevels.Level1)
        {
            Value = (int)armorLevel;
        }
    }
}
