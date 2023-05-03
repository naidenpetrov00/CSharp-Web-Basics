namespace Andreys.Models
{
	using Andreys.Data.Enums;
	using System.ComponentModel.DataAnnotations;

	public partial class Product
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(20)]
		public string Name { get; set; }

		[MaxLength(20)]
		public string Description { get; set; }

		public string ImageUrl { get; set; }

		public decimal Price { get; set; }

		[Required]
		public CategoryType Category { get; set; }

		[Required]
		public GenderType Gender { get; set; }

		public string UserId { get; set; }

		public User User { get; set; }
	}
}
