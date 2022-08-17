using SportsStore.Models;

namespace SportsStore.Data.ViewModels
{
    public class NewProductDropdownsVM
    {
        public NewProductDropdownsVM()
        {
            Suppliers = new List<Supplier>();
            Categories = new List<Category>();
        }

        public List<Supplier> Suppliers { get; set; }
        public List<Category> Categories { get; set; }
    }
}
