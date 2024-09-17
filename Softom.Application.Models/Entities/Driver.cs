using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }

        [ForeignKey("Member")]
        public int MemberId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        [ForeignKey("ContactInformation")]
        public int ContactInformationId { get; set; }
        public bool ValidLicense { get; set; }
        public string? LicenseCode { get; set; }
        public DateTime LicenseExpiryDate { get; set; } = DateTime.Now;
		public DateTime Createddate { get; set; } = DateTime.Now;
        public DateTime Modifieddate { get; set; } = DateTime.Now;
        public bool Isdeleted { get; set; }
        public Member? Member { get; set; }
        public Address? Address { get; set; }
        public Vehicle? Vehicle { get; set; }
        public ContactInformation? ContactInformation { get; set; }
    }
}
