using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;

namespace PrimitiveTextGame.Telegram.Modules.Games.Models;

public class Armor : EntityBase<Guid>
{
    public Armor(ArmorLevel armorLevel, string name, ArmorType armorType)
    {
        ArmorLevel = armorLevel;
        Name = name;
        ArmorType = armorType;
    }
    
    public ArmorLevel ArmorLevel { get; set; }
    public string Name { get; set; }
    public ArmorType ArmorType { get; set; }
    public List<User> Users { get; set; }
}