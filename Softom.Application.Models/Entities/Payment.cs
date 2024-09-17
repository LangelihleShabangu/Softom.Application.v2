using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Softom.Application.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        [ForeignKey("PaymentType")]
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public DateTime PaymentDate { get; set; }= DateTime.Now;
        public DateTime Createddate { get; set; } = DateTime.Now;
        public DateTime Modifieddate { get; set; } = DateTime.Now;
        public bool Isdeleted { get; set; }
        public Status Status { get; set; }
        public Member Member { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
