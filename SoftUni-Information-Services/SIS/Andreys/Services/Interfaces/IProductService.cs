using Andreys.ViewModels;

namespace Andreys.Services.Interfaces
{
	public interface IProductService
	{
		void AddProduct(ProductInputModel input);
		void DeleteProduct(string query);
		ProductDetailsViewModel GetProductDetails(string querryData);
		string GetProductId(string querryData);
	}
}