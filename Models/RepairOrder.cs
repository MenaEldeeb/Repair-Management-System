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

        // تكلفة الإصلاح
        public decimal Cost { get; set; }


        // =========================
        // DEVICE
        // =========================

        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }


        // =========================
        // TECHNICIAN
        // =========================

        public int TechnicianId { get; set; }

        [ForeignKey("TechnicianId")]
        public Technician? Technician { get; set; }


        // =========================
        // PAYMENTS
        // =========================

        public List<Payment> Payments { get; set; }
            = new List<Payment>();
    }
}