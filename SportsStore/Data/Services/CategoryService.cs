using Microsoft.EntityFrameworkCore;
using SportsStore.Data.Base;
using SportsStore.Models;

namespace SportsStore.Data.Services
{
    public class CategoryService : EntityBaseRepository<Category>, ICategoryService
    {
        public CategoryService(AppDbContext context) : base(context) { }
    }
}
