using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace FinalProject.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string DeviceType { get; set; } = string.Empty;

        [MaxLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Problem { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public List<RepairOrder> RepairOrders { get; set; }
            = new List<RepairOrder>();
    }
}