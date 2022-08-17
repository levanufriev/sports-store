using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public List<Product> Products { get; set; }
    }
}
