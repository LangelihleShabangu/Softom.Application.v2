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
            Payment = new Payment();
            Association = new Association();
        }
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public Member? Member { get; set; }
        public Association? Association { get; set; }
    }
}
