using SportsStore.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class NewProductVM
    {
        public int Id { get; set; }

        [Display(Name = "Product name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Product description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Price in $")]
        [Required(ErrorMessage = "Price is required")]
        [Range(typeof(decimal), "0.00", "1999.99")]
        public decimal Price { get; set; }

        [Display(Name = "Product image")]
        [Required(ErrorMessage = "Product image is required")]
        public string ImageUrl { get; set; }

        [Display(Name = "Select a category")]
        [Required(ErrorMessage = "Movie category is required")]
        public int CategoryId { get; set; }


        [Display(Name = "Select a brand")]
        [Required(ErrorMessage = "Brand is required")]
        public int SupplierId { get; set; }
    }
}
