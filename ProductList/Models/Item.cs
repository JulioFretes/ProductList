
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductList.Models
{
    [Table("Tb_Item")]
    public class Item
    {
        public int Id { get; set; }

        [Required, MaxLength(200), Display(Name = "Item Name")]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public int Quantity { get; set; }

        [Required]
        public bool Available { get; set; }

        public decimal Price { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
