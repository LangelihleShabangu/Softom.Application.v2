using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? VinNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime DiskExpiryDate { get; set; } = DateTime.Now;
        public DateTime Createddate { get; set; } = DateTime.Now;
        public DateTime Modifieddate { get; set; } = DateTime.Now;
        public bool Isdeleted { get; set; }
    }
}
