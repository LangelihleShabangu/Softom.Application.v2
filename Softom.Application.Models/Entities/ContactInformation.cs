using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class ContactInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactInformationId { get; set; }
        public string? Notes { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CellNumber { get; set; }
        public string? EmailAddress { get; set; } = "None";
        public DateTime Createddate { get; set; } = DateTime.UtcNow;
        public DateTime Modifieddate { get; set; } = DateTime.UtcNow;
        public bool Isdeleted { get; set; }
    }
}
