using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobile_de_Casa.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter value greater than or equal to 1")]
        public int Count { get; set; }
    }
}
