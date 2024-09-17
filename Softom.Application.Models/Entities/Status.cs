using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        public string? Name { get; set; }
        public DateTime Createddate { get; set; } = DateTime.UtcNow;
        public DateTime Modifieddate { get; set; } = DateTime.UtcNow;
        public bool Isdeleted { get; set; }
    }
}
