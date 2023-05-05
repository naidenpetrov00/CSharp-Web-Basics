namespace Andreys.Services
{
	using Andreys.Data;
	using Andreys.Data.Enums;
	using Andreys.Models;
	using Andreys.Services.Interfaces;
	using Andreys.ViewModels;

	public class ProductService : IProductService
	{
		private readonly ApplicationDbContext db;

		public ProductService(ApplicationDbContext db)
		{
			this.db = db;
		}

		public void AddProduct(ProductInputModel input)
		{
			var product = new Product
			{
				Name = input.Name,
				Description = input.Description,
				ImageUrl = input.ImageUrl,
				Category = Enum.Parse<CategoryType>(input.Category),
				Gender = Enum.Parse<GenderType>(input.Gender),
				Price = input.Price,
			};

			db.Products.Add(product);
			db.SaveChanges();
		}
	}
}
