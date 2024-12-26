using Microsoft.AspNetCore.Builder;
using PrimitiveTextGame.Telegram.Modules.Games.Abstractions;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Data;

public static class DataSeed
{
	public static async Task<IServiceCollection> SeedDataAsync(this IServiceCollection services, IServiceScopeFactory serviceScopeFactory)
	{
		await using var scope = serviceScopeFactory.CreateAsyncScope();
		var characterRepository  = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
		var armorRepository  = scope.ServiceProvider.GetRequiredService<IArmorRepository>();
		var weaponRepository  = scope.ServiceProvider.GetRequiredService<ICharacterRepository>();
		
		//Characters
		var Knight = new Character(CharacterType.Knight, Enum.GetName(CharacterType.Knight));
		var Lumberjack = new Character(CharacterType.Lumberjack, Enum.GetName(CharacterType.Lumberjack));
		var Mage = new Character(CharacterType.Mage, Enum.GetName(CharacterType.Mage));
		
		characterRepository.Create(Knight);
		characterRepository.Create(Lumberjack);
		characterRepository.Create(Mage);
		
		//Armors
		List<Armor> armors = new List<Armor>();
		ArmorLevel[] armorLevels = [ArmorLevel.Light, ArmorLevel.Medium, ArmorLevel.Heavy];
		ArmorType[] armorTypes = [
			ArmorType.DaggerArmor,
			ArmorType.AxeArmor,
			ArmorType.FireArmor,
			ArmorType.KnifeArmor,
			ArmorType.LightningArmor,
			ArmorType.LogArmor,
			ArmorType.SpearArmor,
			ArmorType.SwordArmor,
			ArmorType.BareHandsArmor];

		foreach (var armorType in armorTypes)
		{
			foreach (var armorLevel in armorLevels)
			{
				armors.Add(new Armor(armorLevel, Enum.GetName(armorType), armorType));
			}			
		}
		armorRepository.Create(armors);
		
		
		//common call for all changes
		await characterRepository.SaveChangesAsync();
		
		return services;
	}
}