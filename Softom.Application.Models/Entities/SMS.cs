using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class SMS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SMSId { get; set; }
        [ForeignKey("Association")]
        public int AssociationId { get; set; }
        public int Total { get; set; }
        public int Balance { get; set; }
        public int Sent { get; set; }
        public string? Notes { get; set; }
        public DateTime Createddate { get; set; } = DateTime.UtcNow;
        public DateTime Modifieddate { get; set; } = DateTime.UtcNow;
        public bool Isdeleted { get; set; }
        public Association Association { get; set; }
    }
}
