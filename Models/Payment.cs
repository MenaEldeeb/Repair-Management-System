
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string PaymentMethod { get; set; } = string.Empty;

        public int RepairOrderId { get; set; }

        [ForeignKey("RepairOrderId")]
        public RepairOrder? RepairOrder { get; set; }
    }
}


