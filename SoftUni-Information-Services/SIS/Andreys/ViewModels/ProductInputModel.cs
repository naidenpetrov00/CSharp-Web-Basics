namespace Andreys.ViewModels
{
	using Andreys.Data.Enums;
	using Andreys.Models;

	public class ProductInputModel
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		public string Category { get; set; }

		public string Gender { get; set; }

		public int Price { get; set; }
	}
}
