using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int MemberId { get; set; }
        public byte[]? MemberImage { get; set; }

        [ForeignKey("Address")]
		public int? AddressId { get; set; }		
		[ForeignKey("Association")]
		public int? AssociationId { get; set; }
		[ForeignKey("ContactInformation")]
		public int? ContactInformationId { get; set; }
		public string? Notes { get; set; }
		public DateTime Createddate { get; set; } = DateTime.Now;
        public DateTime Modifieddate { get; set; } = DateTime.Now;
        public bool Isdeleted { get; set; }
        public Address? Address { get; set; }        
		public Association? Association { get; set; }
		public ContactInformation? ContactInformation { get; set; }
	}
}
