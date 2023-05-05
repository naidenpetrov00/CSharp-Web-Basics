namespace Andreys.Services
{
	using Andreys.Data;
	using Andreys.Services.Interfaces;
	using Andreys.ViewModels;

	public class HomeService : IHomeService
	{
		private readonly ApplicationDbContext db;

		public HomeService(ApplicationDbContext db)
		{
			this.db = db;
		}

		public ICollection<ProductViewModel> GetProducts()
		{
			var productModels = db.Products
				.Select(p => new ProductViewModel
				{
					Id= p.Id,
					Name = p.Name,
					Price = p.Price,
					ImageUrl = p.ImageUrl,
				})
				.ToList();

			return productModels;
		}
	}
}
