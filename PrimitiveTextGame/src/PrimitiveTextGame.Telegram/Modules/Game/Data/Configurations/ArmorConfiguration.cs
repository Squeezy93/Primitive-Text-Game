using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimitiveTextGame.Telegram.Modules.Game.Models;

namespace PrimitiveTextGame.Telegram.Modules.Game.Data.Configurations;

public class ArmorConfiguration : IEntityTypeConfiguration<Armor>
{
	public void Configure(EntityTypeBuilder<Armor> builder)
	{
		throw new NotImplementedException();
	}
}