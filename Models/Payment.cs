using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20)]
        public string PaymentMethod { get; set; } = "Cash";

        public int RepairOrderId { get; set; }

        [ForeignKey("RepairOrderId")]
        public RepairOrder? RepairOrder { get; set; }
    }
}