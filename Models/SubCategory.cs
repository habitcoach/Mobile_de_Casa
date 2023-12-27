using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobile_de_Casa.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Mobile")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [Display(Name = "Brand")]
        public virtual Category Category { get; set; }
    }
}
