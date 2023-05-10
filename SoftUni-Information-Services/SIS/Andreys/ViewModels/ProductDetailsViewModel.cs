namespace Andreys.ViewModels
{
	using Andreys.Data.Enums;
	using Andreys.Models;

	public class ProductDetailsViewModel
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public GenderType Gender { get; set; }

		public CategoryType Category { get; set; }

		public string ImageUrl { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }
	}
}
