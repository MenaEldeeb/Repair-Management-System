using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class RepairOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RepairOrderId { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }

        public int TechnicianId { get; set; }

        [ForeignKey("TechnicianId")]
        public Technician? Technician { get; set; }

        public List<Payment> Payments { get; set; }
            = new List<Payment>();
    }
}
