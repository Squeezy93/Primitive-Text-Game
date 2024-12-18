using Microsoft.EntityFrameworkCore;
using PrimitiveTextGame.Telegram.Modules.Games.Models;

namespace PrimitiveTextGame.Telegram.Modules.Games.Data;

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
	public DbSet<Armor> Armors { get; set; }
	public DbSet<Character> Characters { get; set; }
	public DbSet<History> Histories { get; set; }
	public DbSet<Weapon> Weapons { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDataContext).Assembly);
	}
}