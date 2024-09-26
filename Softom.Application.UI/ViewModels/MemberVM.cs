using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using Softom.Application.Models.MV;

namespace Softom.Application.UI.ViewModels
{
    public class MemberVM
    {
        public MemberVM()
        {
            Payment = new Payment();
            MemberList = new List<Member>();
            paymentDetailsVM =  new PaymentDetails();
        }
        public Payment? Payment { get; set; }
        public PaymentDetails? paymentDetailsVM { get; set; }
        public IEnumerable<Member> MemberList { get; set; }


        public int MemberId { get; set; }

        [BindProperty(Name = "PaymentTypeId")]
        public int PaymentTypeId { get; set; }

        public PaymentType? PaymentType { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? PaymentTypeList { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? PaymentStatusList { get; set; }
    }
}
