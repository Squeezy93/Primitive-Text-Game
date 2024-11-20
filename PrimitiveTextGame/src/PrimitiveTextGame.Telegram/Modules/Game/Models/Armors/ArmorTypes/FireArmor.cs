namespace PrimitiveTextGame.Telegram.Modules.Game.Models.Armors.ArmorTypes;

public class FireArmor : IArmor
{
    public Guid Id { get; set; }
    public int Value { get; set; }
    public string Name { get; private set; } = "Fire Armor";

    public FireArmor(ArmorLevels armorLevel = ArmorLevels.Level1)
    {
        Value = (int)armorLevel;
    }
}