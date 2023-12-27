using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobile_de_Casa.Models
{
    public class OrderMaster
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public double OrderTotalOriginal { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DisplayName("Order Total")]
        public double OrderTotal { get; set; }
        [Required]
        [DisplayName("Pickup Time")]
        public DateTime PickUpTime { get; set; }
        [Required]
        [NotMapped]
        [DisplayName("Pickup Date")]
        public DateTime PickUpDate { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string Comments { get; set; }
        [DisplayName("Pickup Name")]
        public string PickUpName { get; set; }
        [DisplayName("Phone")]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public string TransactionId { get; set; }

    }
}
