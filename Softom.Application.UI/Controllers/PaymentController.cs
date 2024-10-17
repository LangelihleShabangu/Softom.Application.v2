using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Softom.Application.BusinessRules.Services.Implementation;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using Softom.Application.Models.MV;
using Softom.Application.UI.ViewModels;
using System.Globalization;
using System.Security.Claims;

namespace Softom.Application.UI.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentServices _PaymentService;
        private readonly IMemberService _MemberService;
        private readonly IPaymentTypeService _PaymentTypeService;
        private readonly IAssociationService _AssociationService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PaymentController(IPaymentServices PaymentService, IMemberService memberService, IPaymentTypeService PaymentTypeService, IAssociationService AssociationService,
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
                MemberList = _MemberService.GetAllMember().Select(u => new SelectListItem
                {
                    Text = u.ContactInformation.Firstname + " " + u.ContactInformation.Surname,
                    Value = u.MemberId.ToString()
                }),
                PaymentTypeList = _PaymentTypeService.GetAllPaymentType().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.PaymentTypeId.ToString()
                }),
                PaymentList = _PaymentService.GetAllPayment().Where(f=>f.Createddate >= Convert.ToDateTime("2024-09-20") && f.Createddate <= DateTime.Now.AddDays(1)).ToList(),
            };

            foreach(var item in paymentDetailsVM.PaymentList)
            {
                item.Member.ContactInformation = _MemberService.GetMemberById(item.MemberId).ContactInformation;
            }

            //DateTime startAtMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
            //DateTime startAtSunday = startAtMonday.AddDays(6);

            //paymentDetailsVM.PaymentList = paymentDetailsVM.PaymentList.Where(f=>f.Createddate >= startAtMonday && f.Createddate <= startAtSunday).ToList();

            return View(paymentDetailsVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("/Payment/MemberPaymentDetails/{PaymentId}")]
        public IActionResult MemberPaymentDetails(int PaymentId)
        {
            var invoiceVM = new InvoiceVM();
            invoiceVM.PaymentId = PaymentId;    
            invoiceVM.Payment = _PaymentService.GetPaymentById(PaymentId);
            invoiceVM.Member = _MemberService.GetMemberById(invoiceVM.Payment.MemberId);
            invoiceVM.Association = _AssociationService.GetAssociationById(invoiceVM.Member.AssociationId.Value);
            return View(invoiceVM);
        }

        [HttpGet]
        [Route("/Payment/GetInvoiceDetails/{PaymentId}")]
        public IActionResult GetInvoiceDetails(int PaymentId)
        {
            var invoiceVM = new InvoiceVM();
            invoiceVM.Payment = _PaymentService.GetPaymentById(PaymentId);
            invoiceVM.Member = _MemberService.GetMemberById(invoiceVM.Payment.MemberId);
            invoiceVM.Association = _AssociationService.GetAssociationById(invoiceVM.Member.AssociationId.Value);
            var byteInfoStatement = new Softom.Application.BusinessRules.Generate_PDF.CreateInvoicePDF().GeneratePDFFile(invoiceVM).ToArray();
            return File(byteInfoStatement, "APPLICATION/pdf", "Payment_" + System.DateTime.Now.ToString("dd MMMM yyyy") + "_" + invoiceVM.Member.ContactInformation.Firstname + "_" + invoiceVM.Member.ContactInformation.Surname + ".pdf");
        }       
        
        [HttpPost]
        public IActionResult CreatePayment(int Member, int PaymentType, decimal Amount)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            var payment = new Payment();
            payment.PaymentStatusId = 1; /* Active */
            payment.MemberId = Member;
            payment.PaymentTypeId = PaymentType;
            payment.Amount = Amount;
            payment.Notes = userId.ToString();
            payment.Createddate = DateTime.Now; 
            payment.PaymentDate = DateTime.Now; 
            payment.Modifieddate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _PaymentService.CreatePayment(payment);
                TempData["success"] = "The Payment has been created successfully.";
                var invoiceVM = new InvoiceVM();
                invoiceVM.Payment = _PaymentService.GetPaymentById(payment.PaymentId);
                invoiceVM.Member = _MemberService.GetMemberById(invoiceVM.Payment.MemberId);

                invoiceVM.Association = _AssociationService.GetAssociationById(invoiceVM.Member.AssociationId.Value);
                var byteInfoStatement = new Softom.Application.BusinessRules.Generate_PDF.CreateInvoicePDF().GeneratePDFFile(invoiceVM).ToArray();
                return File(byteInfoStatement, "APPLICATION/pdf", "Payment_" + System.DateTime.Now.ToString("dd MMMM yyyy") + "_" + invoiceVM.Member.ContactInformation.Firstname + "_" + invoiceVM.Member.ContactInformation.Surname + ".pdf");
            }

            return Json(new { success = true });
        }

        public IActionResult Update(int PaymentId)
        {
            Payment? obj = _PaymentService.GetPaymentById(PaymentId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Payment obj)
        {
            if (ModelState.IsValid && obj.PaymentId > 0)
            {

               _PaymentService.UpdatePayment(obj);
                TempData["success"] = "The Payment has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }       


        [HttpGet]        
        public IActionResult Daily()
        {
            List<Payment> payments = new List<Payment>();
            var obj = _PaymentService.GetAllPayment().GroupBy(f=>f.Createddate.ToString("dd MMMM yyyy"));
            var PaymentList = from x in _PaymentService.GetAllPayment()
                      group x by x.Createddate.ToString("dd MMMM yyyy");
            var PaymentResults = from y in PaymentList
                         select new
                         {
                             Id = y.Key,
                             Amount = y.Sum(x => x.Amount)
                         };
            
            foreach(var item in PaymentResults)
            {
                payments.Add(new Payment() { Createddate = Convert.ToDateTime(item.Id), Amount = item.Amount });
            }

            var paymentDetails = new PaymentDetails();
            paymentDetails.PaymentList = payments;

            return View(paymentDetails);
        }

        //Softom.Application.Models.MV
        [HttpGet]
        public IActionResult GetPaymentDetails(PaymentDetails paymentDetails)
        {
            List<Payment> payments = new List<Payment>();
            var PamentMadeList = _PaymentService.GetAllPayment().Where(f =>
                f.PaymentDate >= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[0]) &&
                f.PaymentDate <= Convert.ToDateTime(paymentDetails.PaymentDate.Split("-")[1])).ToList();

            paymentDetails.PaymentsMade = PamentMadeList;

            var qry = _PaymentService.GetAllPayment().SelectMany
            (
                foo => _MemberService.GetAllMember().Where(bar => foo.MemberId == bar.MemberId).DefaultIfEmpty(),
                (foo, bar) => new
                {
                    Foo = foo,
                    Bar = bar
                }
            ).ToList();

            return View(paymentDetails);
        }


        [HttpGet]
        [Route("/Payment/Delete/{PaymentId}")]
        public IActionResult Delete(int PaymentId)
        {
            Payment? obj = _PaymentService.GetPaymentById(PaymentId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                _PaymentService.DeletePayment(PaymentId);
                TempData["success"] = "The Payment has been removed successfully.";
                return RedirectToAction(nameof(Index));
            }           
        }


        [HttpPost]
        public IActionResult Delete(Payment obj)
        {
         bool deleted = _PaymentService.DeletePayment(obj.PaymentId);
            if (deleted)
            {
                TempData["success"] = "The Payment has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the Payment.";
            }
            return View();
        }
    }
}
