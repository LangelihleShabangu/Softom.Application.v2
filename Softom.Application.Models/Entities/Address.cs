using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public int Code { get; set; }
        public DateTime Createddate { get; set; } = DateTime.UtcNow;
        public DateTime Modifieddate { get; set; } = DateTime.UtcNow;
        public bool Isdeleted { get; set; }
    }
}
