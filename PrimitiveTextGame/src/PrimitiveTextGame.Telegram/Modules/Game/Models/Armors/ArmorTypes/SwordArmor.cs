namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors.ArmorTypes
{
    public class SwordArmor : IArmor
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Name { get; private set; } = "Sword Armor";

        public SwordArmor(ArmorLevels armorLevel = ArmorLevels.Level1)
        {
            Value = (int)armorLevel;
        }
    }
}
