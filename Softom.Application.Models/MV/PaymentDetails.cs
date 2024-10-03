using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Models.MV
{
    public class PaymentDetails
    {
        public PaymentDetails()
        {
            Payment = new Payment();
            PaymentList = new List<Payment>();
            PaymentsMade = new List<Payment>();
            PaymentsNotMade = new List<Payment>();
        }
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public List<Payment>? PaymentList { get; set; }

        public List<Payment>? PaymentsMade { get; set; }
        public List<Payment>? PaymentsNotMade { get; set; }

        public string PaymentDate { get; set; }

        public PaymentDetails? paymentDetailsVM { get; set; }

        public Member? Member { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? MemberList { get; set; }        
        public PaymentType? PaymentType { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? PaymentTypeList { get; set; }
        public string? PaymentStatus { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? PaymentStatusList { get; set; }
        public string? Status { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? StatusList { get; set; }
    }
}
