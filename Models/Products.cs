using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobile_de_Casa.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public double Price { get; set; }
        [Display(Name = "Back case")]
        public string Image { get; set; }
        [Display(Name = "Brand")]
        public int? CategoryId { get; set; }
        [Display(Name = "Mobile")]
        public int? SubCategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [Display(Name = "Brand")]
        public virtual Category Category { get; set; }
        [ForeignKey("SubCategoryId")]
        [Display(Name = "Mobile")]
        public virtual SubCategory SubCategory { get; set; }
    }
}
