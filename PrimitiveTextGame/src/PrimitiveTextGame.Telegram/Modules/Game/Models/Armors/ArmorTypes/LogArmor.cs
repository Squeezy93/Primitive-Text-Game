namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors.ArmorTypes
{
    public class LogArmor : IArmor
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Name { get; private set; } = "Log Armor";

        public LogArmor(ArmorLevels armorLevel = ArmorLevels.Level1)
        {
            Value = (int)armorLevel;
        }
    }
}
