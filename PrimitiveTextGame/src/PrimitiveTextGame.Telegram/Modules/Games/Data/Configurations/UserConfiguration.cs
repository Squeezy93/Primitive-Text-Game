using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserTelegramId).IsRequired();
        builder.Property(x => x.UserName);
        builder.Property(x => x.CharacterId);
        builder.Property(x => x.CreateDate).IsRequired();
        builder.Property(x => x.DeleteDate);

        builder.HasIndex(x => x.UserTelegramId);
        builder.HasIndex(x => x.UserName);

        builder.HasMany(x => x.Weapons)
            .WithMany(x => x.Users)
            .UsingEntity(join => join.ToTable("UserWeapons"));

        builder.HasMany(x => x.Games)
            .WithMany(x => x.Users)
            .UsingEntity(join => join.ToTable("UserGames"));
    }
}