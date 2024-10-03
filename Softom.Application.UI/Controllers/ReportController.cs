using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using Softom.Application.Models.MV;

namespace Softom.Application.UI.Controllers
{
    public class ReportController : Controller
    {
        private readonly IPaymentServices _PaymentService;
        private readonly IMemberService _MemberService;
        private readonly IPaymentTypeService _PaymentTypeService;
        private readonly IAssociationService _AssociationService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ReportController(IPaymentServices PaymentService, IMemberService memberService, IPaymentTypeService PaymentTypeService, IAssociationService AssociationService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _PaymentService = PaymentService;
            _MemberService = memberService;
            _PaymentTypeService = PaymentTypeService;
            _AssociationService = AssociationService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            PaymentDetails paymentDetails = new PaymentDetails();
            return View(paymentDetails);
        }

        [HttpPost]
        public IActionResult GetPaymentDetails(PaymentDetails paymentDetails)
        {
            List<Payment> payments = new List<Payment>();
            var PamentMadeList = _PaymentService.GetAllPayment().Where(f =>
                f.PaymentDate >= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]) &&
                f.PaymentDate <= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1])).ToList();
            
            var qry = _MemberService.GetAllMember().SelectMany
            (
                foo => PamentMadeList.Where(bar => foo.MemberId == bar.MemberId).DefaultIfEmpty(),
                (foo, bar) => new
                {
                    Foo = foo,
                    Bar = bar
                }).ToList();

            foreach (var item in qry)
            {
                if (item.Bar != null)
                {
                    paymentDetails.PaymentsMade.Add(new Application.Models.Payment() { Amount = item.Bar.Amount, Member = item.Foo, PaymentDate = item.Bar.PaymentDate, Notes = item.Bar.PaymentType.Name });
                }
                else
                {
                    paymentDetails.PaymentsNotMade.Add(new Application.Models.Payment() { Member = item.Foo });
                }
            }

            return View("Index",paymentDetails);
        }
    }
}
