namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors.ArmorTypes
{
    public class KnifeArmor : IArmor
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Name { get; private set; } = "Knife Armor";

        public KnifeArmor(ArmorLevels armorLevel = ArmorLevels.Level1)
        {
            Value = (int)armorLevel;
        }
    }
}
