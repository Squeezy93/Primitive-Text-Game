using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrimitiveTextGame.Telegram.Modules.Games.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Models.Game>
{
    public void Configure(EntityTypeBuilder<Models.Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreateDate).IsRequired();
        builder.Property(x => x.DeleteDate);
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(x => x.WinnerId);

        builder.HasIndex(x => x.WinnerId);

        builder.HasMany(x => x.Histories)
            .WithOne(x => x.Game)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Winner)
            .WithMany()
            .HasForeignKey(x => x.WinnerId);
    }
}