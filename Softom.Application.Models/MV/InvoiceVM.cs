using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Models.MV
{
    public class InvoiceVM
    {
        public InvoiceVM()
        {
            Member = new Member();
            MemberList = new List<Member>();
            Payment = new Payment();
            Association = new Association();
        }

        public string ReportDate { get; set; }
        public int PaymentId { get; set; }
        public List<Payment>? PaymentList { get; set; }
        public Payment? Payment { get; set; }
        public Member? Member { get; set; }
        public List<Member>? MemberList { get; set; }
        public Association? Association { get; set; }
    }
}
