namespace DemoApp
{
	using Microsoft.EntityFrameworkCore;
	public class ApplicationDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-ERAGAG4\\SQLEXPRESS;Database=DemoApp;Integrated Security=True;Encrypt=false");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}

		public DbSet<Tweet> Tweets { get; set; }
	}
}
 