using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Game.Models;

namespace PrimitiveTextGame.Telegram.Modules.Game.Data;

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
	public DbSet<User> Armors { get; set; }
	public DbSet<User> Characters { get; set; }
	public DbSet<User> Histories { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDataContext).Assembly);
	}
}