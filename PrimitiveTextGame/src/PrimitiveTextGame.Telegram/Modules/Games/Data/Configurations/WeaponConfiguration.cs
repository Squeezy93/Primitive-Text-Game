using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Data.Configurations;

public class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
{
    public void Configure(EntityTypeBuilder<Weapon> builder)
    {
        builder.ToTable("Weapons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.CharacterType).IsRequired();
        builder.Property(x => x.Damage).IsRequired();
        builder.Property(x => x.CreateDate).IsRequired();
        builder.Property(x => x.DeleteDate);

        builder.HasIndex(x => x.Name);

        /*builder.HasMany(x => x.Users)
            .WithMany(x => x.Weapons)
            .UsingEntity(join => join.ToTable("UserWeapons"));*/
    }
}