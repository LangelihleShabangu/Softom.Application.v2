using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Member_Vehicle
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Member_VehicleId { get; set; }

		[ForeignKey("Vehicle")]
		public int VehicleId { get; set; }
		[ForeignKey("Member")]
		public int MemberId { get; set; }
		public string? Notes { get; set; }
		public DateTime Createddate { get; set; } = DateTime.UtcNow;
        public DateTime Modifieddate { get; set; } = DateTime.UtcNow;
        public bool Isdeleted { get; set; }
    }
}
