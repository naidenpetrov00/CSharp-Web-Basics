namespace SulsApp
{
	using SulsApp.Models;
	using Microsoft.EntityFrameworkCore;

	public class ApplicationDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-ERAGAG4\\SQLEXPRESS;Database=SulsApp;Integrated Security=True;Encrypt=false");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Problem> Problems { get; set; }
		public DbSet<Submission> Submissions { get; set; }
	}
}
