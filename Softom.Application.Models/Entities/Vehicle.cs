using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public string? Registration { get; set; }        
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? VINNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime DiskExpiryDate { get; set; } = DateTime.Now;
        public DateTime Createddate { get; set; } = DateTime.Now;
        public DateTime Modifieddate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; } = Guid.Empty;
        public Guid ModifiedBy { get; set; } = Guid.Empty;
        public bool Isdeleted { get; set; }
        public Member Member { get; set; }
        public Status Status { get; set; }        
    }
}
