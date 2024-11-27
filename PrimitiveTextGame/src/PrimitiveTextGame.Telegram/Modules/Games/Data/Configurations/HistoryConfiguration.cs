using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Data.Configurations;

public class HistoryConfiguration : IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        builder.ToTable("Histories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Order).IsRequired();
        builder.Property(x => x.Damage).IsRequired();
        builder.Property(x => x.WeaponId).IsRequired();
        builder.Property(x => x.Health).IsRequired();
        builder.Property(x => x.CreateDate).IsRequired();
        builder.Property(x => x.DeleteDate);

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.Weapon)
            .WithMany()
            .HasForeignKey(x => x.WeaponId);
    }
}