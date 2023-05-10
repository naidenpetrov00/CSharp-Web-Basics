namespace Andreys.Services
{
	using Andreys.Data;
	using Andreys.Models;
	using Andreys.Data.Enums;
	using Andreys.ViewModels;
	using Andreys.Services.Interfaces;

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

		public void DeleteProduct(string query)
		{
			var productId = GetProductId(query);
			var product = db.Products
				.FirstOrDefault(p => p.Id == productId);
			db.Products.Remove(product);
			db.SaveChanges();
		}

		public ProductDetailsViewModel GetProductDetails(string querryData)
		{
			var productId = this.GetProductId(querryData);
			var product = db.Products
				.FirstOrDefault(p => p.Id == productId);

			var productViewModel = new ProductDetailsViewModel
			{
				Id = product.Id,
				Name = product.Name,
				Gender = product.Gender,
				Category = product.Category,
				ImageUrl = product.ImageUrl,
				Description = product.Description,
				Price = product.Price,
			};

			return productViewModel;
		}

		public string GetProductId(string querryData)
		{
			//id={value}
			return querryData
				.Substring(querryData.IndexOf('=') + 1);
		}
	}
}
