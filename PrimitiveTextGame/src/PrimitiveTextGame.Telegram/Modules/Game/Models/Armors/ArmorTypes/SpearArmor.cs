namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors.ArmorTypes
{
    public class SpearArmor : IArmor
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Name { get; private set; } = "Spear Armor";

        public SpearArmor(ArmorLevels armorLevel = ArmorLevels.Level1)
        {
            Value = (int)armorLevel;
        }
    }
}
