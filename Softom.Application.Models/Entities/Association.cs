using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Association
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssociationId { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public string? AssociationName { get; set; }
        public string? Notes { get; set; }
        public string? Website { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CellNumber { get; set; }
        public string? EmailAddress { get; set; }
		public byte[]? Logo { get; set; }
        public DateTime Createddate { get; set; } = DateTime.Now;
        public DateTime Modifieddate { get; set; } = DateTime.Now;
        public bool Isdeleted { get; set; }
        public Address? Address { get; set; }
    }
}
