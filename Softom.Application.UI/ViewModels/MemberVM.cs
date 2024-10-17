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
            Member = new Member();
            Payment = new Payment();            
            MemberList = new List<Member>();
            VehicleList= new List<Vehicle>();
            paymentDetailsVM =  new PaymentDetails();
        }

        public Member? Member { get; set; }
        public Vehicle? Vehicle { get; set; }
        public Payment? Payment { get; set; }
        public PaymentDetails? paymentDetailsVM { get; set; }
        public IEnumerable<Member> MemberList { get; set; }
        public IEnumerable<Vehicle> VehicleList { get; set; }
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
