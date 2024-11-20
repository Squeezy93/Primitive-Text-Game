namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors.ArmorTypes
{
    public class LightningArmor : IArmor
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Name { get; private set; } = "Lightning Armor";

        public LightningArmor(ArmorLevels armorLevel = ArmorLevels.Level1)
        {
            Value = (int)armorLevel;
        }
    }
}
