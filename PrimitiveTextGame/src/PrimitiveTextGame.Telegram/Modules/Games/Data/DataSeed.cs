using PrimitiveTextGame.Telegram.Modules.Games.Abstractions.Repositories;
using PrimitiveTextGame.Telegram.Modules.Games.Implementations.Specifications;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Data;

public static class DataSeed 
{
    public static async Task SeedDataAsync(
        ICharacterRepository characterRepository,
        IArmorRepository armorRepository,
        IWeaponRepository weaponRepository)
    {

        var characterExists = await characterRepository.IsExists(new AnyEntityExistsSpecification<Character>());
        if (!characterExists)
        {
            List<Character> characters =
            [
                new Character(CharacterType.Knight, Enum.GetName(CharacterType.Knight)),
                new Character(CharacterType.Lumberjack, Enum.GetName(CharacterType.Lumberjack)),
                new Character(CharacterType.Mage, Enum.GetName(CharacterType.Mage))
            ];
            characterRepository.Create(characters);
        }

        var armorExists = await armorRepository.IsExists(new AnyEntityExistsSpecification<Armor>());
        if (!armorExists)
        {
            List<Armor> armors = new List<Armor>();
            ArmorLevel[] armorLevels = [ArmorLevel.Light, ArmorLevel.Medium, ArmorLevel.Heavy];
            ArmorType[] armorTypes = 
            [
                ArmorType.DaggerArmor,
                ArmorType.AxeArmor,
                ArmorType.FireArmor,
                ArmorType.KnifeArmor,
                ArmorType.LightningArmor,
                ArmorType.LogArmor,
                ArmorType.SpearArmor,
                ArmorType.SwordArmor,
                ArmorType.BareHandsArmor
            ];
            foreach (var armorType in armorTypes)
            {
                foreach (var armorLevel in armorLevels)
                {
                    armors.Add(new Armor(armorLevel, Enum.GetName(armorType), armorType));
                }
            }
            armorRepository.Create(armors);
        }

        var weaponExists = await weaponRepository.IsExists(new AnyEntityExistsSpecification<Weapon>());
        if (!weaponExists)
        {
            List<Weapon> weapons =
            [
                new Weapon("Knife", CharacterType.Knight, 20),
                new Weapon("Spear", CharacterType.Knight, 25),
                new Weapon("Sword", CharacterType.Knight, 30),
                new Weapon("Axe", CharacterType.Lumberjack, 30),
                new Weapon("Barehands", CharacterType.Lumberjack, 15),
                new Weapon("Log", CharacterType.Lumberjack, 20),
                new Weapon("Dagger", CharacterType.Mage, 15),
                new Weapon("Fire", CharacterType.Mage, 20),
                new Weapon("Lightning", CharacterType.Mage, 25),
            ];
            weaponRepository.Create(weapons);
        }
        await characterRepository.SaveChangesAsync();
    }
}