using SportsStore.Models;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data.ViewModels;
using SportsStore.Data.Base;
using Microsoft.Data.SqlClient;

namespace SportsStore.Data.Services
{
    public class ProductService : EntityBaseRepository<Product>, IProductService
    {
        private readonly AppDbContext context;

        public ProductService(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task AddNewProductAsync(NewProductVM data)
        {
            var newProduct = new Product()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageUrl = data.ImageUrl,
                CategoryId = data.CategoryId,
                SupplierId = data.SupplierId
            };
            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();

            await context.SaveChangesAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var productDetails = await context.Products
                .Include(c => c.Category)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(n => n.Id == id);

            return productDetails;
        }

        public async Task<NewProductDropdownsVM> GetNewProductDropdownsValues()
        {
            var response = new NewProductDropdownsVM()
            {
                Suppliers = await context.Suppliers.OrderBy(n => n.Name).ToListAsync(),
                Categories = await context.Categories.OrderBy(n => n.Name).ToListAsync()
            };

            return response;
        }


        public async Task UpdateProductAsync(NewProductVM data)
        {
            var dbProduct = await context.Products.FirstOrDefaultAsync(n => n.Id == data.Id);

            if (dbProduct != null)
            {
                dbProduct.Name = data.Name;
                dbProduct.Description = data.Description;
                dbProduct.Price = data.Price;
                dbProduct.ImageUrl = data.ImageUrl;
                dbProduct.CategoryId = data.CategoryId;
                dbProduct.SupplierId = data.SupplierId;
                await context.SaveChangesAsync();
            }

            await context.SaveChangesAsync();
        }
    }
}

