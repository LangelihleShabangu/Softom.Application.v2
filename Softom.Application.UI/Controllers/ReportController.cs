using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using Softom.Application.Models.MV;
using Stripe;

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
            PaymentDetails paymentDetailsVM = new()
            {
                PaymentTypeList = _PaymentTypeService.GetAllPaymentType().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.PaymentTypeId.ToString()
                })
            };           
            return View(paymentDetailsVM);
        }
        
        [HttpPost]
        public IActionResult GetPaymentDetails_PDF_Paid(PaymentDetails paymentDetails)
        {
            List<Payment> payments = new List<Payment>();
            var PamentMadeList = _PaymentService.GetAllPayment().Where(f =>
                f.PaymentTypeId == paymentDetails.PaymentTypeId &&
                f.PaymentDate >= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]) &&
                f.PaymentDate <= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1])).ToList();

            var invoiceVM = new InvoiceVM();

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
                    if (item.Bar.Amount > 0)
                    {
                        paymentDetails.PaymentsMade.Add(new Application.Models.Payment() { Amount = item.Bar.Amount, Member = item.Foo, PaymentDate = item.Bar.PaymentDate, Notes = item.Bar.PaymentType.Name });
                    }
                }
            }
            invoiceVM.MemberList = invoiceVM.MemberList.Distinct().ToList();
            invoiceVM.Association = _AssociationService.GetAllAssociation().FirstOrDefault();
            invoiceVM.PaymentList = paymentDetails.PaymentsMade.ToList();
            invoiceVM.ReportDate = Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]).ToString("dd MMMM yyyy") + "_" + Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1]).ToString("dd MMMM yyyy");
            var byteInfoStatement = new Softom.Application.BusinessRules.Generate_PDF.CreatePaymentReportPDF_Paid().GeneratePDFFile(invoiceVM).ToArray();
            return File(byteInfoStatement, "APPLICATION/pdf", "Report_" + Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]).ToString("dd MMMM yyyy") + "_" + Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1]).ToString("dd MMMM yyyy") + ".pdf");
        }


        [HttpPost]
        public IActionResult GetPaymentDetails_PDF(PaymentDetails paymentDetails)
        {
            List<Payment> payments = new List<Payment>();
            var PamentMadeList = _PaymentService.GetAllPayment().Where(f =>
                f.PaymentTypeId == paymentDetails.PaymentTypeId &&
                f.PaymentDate >= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]) &&
                f.PaymentDate <= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1])).ToList();

            var invoiceVM = new InvoiceVM();

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
                if (item.Bar == null)
                {
                    invoiceVM.MemberList.Add(item.Foo);
                }               
            }
            invoiceVM.MemberList = invoiceVM.MemberList.Distinct().ToList();
            invoiceVM.Association = _AssociationService.GetAllAssociation().FirstOrDefault();
            invoiceVM.ReportDate = Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]).ToString("dd MMMM yyyy") + "_" + Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1]).ToString("dd MMMM yyyy");
            var byteInfoStatement = new Softom.Application.BusinessRules.Generate_PDF.CreatePaymentReportPDF().GeneratePDFFile(invoiceVM).ToArray();
            return File(byteInfoStatement, "APPLICATION/pdf", "Report_" + Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]).ToString("dd MMMM yyyy") + "_"+ Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1]).ToString("dd MMMM yyyy") + ".pdf");                       
        }

        [HttpPost]
        public IActionResult GetPaymentDetails(PaymentDetails paymentDetails)
        {
            List<Payment> payments = new List<Payment>();
            var PamentMadeList = _PaymentService.GetAllPayment().Where(f =>
                f.PaymentTypeId == paymentDetails.PaymentTypeId &&
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
                    if (item.Bar.Amount > 0)
                    {
                        paymentDetails.PaymentsMade.Add(new Application.Models.Payment() { Amount = item.Bar.Amount, Member = item.Foo, PaymentDate = item.Bar.PaymentDate, Notes = item.Bar.PaymentType.Name });
                    }
                }
                else
                {
                    paymentDetails.PaymentsNotMade.Add(new Application.Models.Payment() { Member = item.Foo });
                }
            }

            paymentDetails.PaymentTypeList = _PaymentTypeService.GetAllPaymentType().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.PaymentTypeId.ToString()
            });

            return View("Index",paymentDetails);
        }
    }
}
