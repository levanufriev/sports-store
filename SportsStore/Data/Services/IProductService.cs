using SportsStore.Data.Base;
using SportsStore.Data.ViewModels;
using SportsStore.Models;

namespace SportsStore.Data.Services
{
    public interface IProductService : IEntityBaseRepository<Product>
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<NewProductDropdownsVM> GetNewProductDropdownsValues();
        Task UpdateProductAsync(NewProductVM data);
        Task AddNewProductAsync(NewProductVM data);
    }
}
