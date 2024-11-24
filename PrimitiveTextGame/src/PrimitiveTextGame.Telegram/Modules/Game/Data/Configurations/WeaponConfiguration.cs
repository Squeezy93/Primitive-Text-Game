using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrimitiveTextGame.Telegram.Modules.Game.Models;

public class WeaponConfiguration : IEntityTypeConfiguration<Armor>
{
	public void Configure(EntityTypeBuilder<Armor> builder)
	{
		throw new NotImplementedException();
	}
}