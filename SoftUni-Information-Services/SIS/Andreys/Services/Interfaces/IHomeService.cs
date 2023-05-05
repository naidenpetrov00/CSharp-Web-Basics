namespace Andreys.Services.Interfaces
{
	using Andreys.ViewModels;

	public interface IHomeService
    {
        ICollection<ProductViewModel> GetProducts();
    }
}