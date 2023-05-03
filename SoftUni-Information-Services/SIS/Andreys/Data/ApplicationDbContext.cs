namespace Andreys.Data
{
	using Andreys.Models;
	using Microsoft.EntityFrameworkCore;

	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-ERAGAG4\\SQLEXPRESS;Database=Andreys;Integrated Security=True;Encrypt=false");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}
