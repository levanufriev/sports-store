using SportsStore.Data.Base;
using SportsStore.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsStore.Models
{
    public class Category : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 chars")]
        public string Name { get; set; }

        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string ImageUrl { get; set; }

        public List<Product> Products { get; set; }

        public int KindOfSportId { get; set; }

        [ForeignKey("KindOfSportId")]
        public KindOfSport KindOfSport { get; set; }
    }
}
